-- oracle_test_setup.sql
-- Script d'initialisation minimal pour tests d'intégration Oracle (BanqueProjet)
-- Usage recommandé :
-- 1) Exécuter en tant que SYSDBA pour créer l'utilisateur TEST :
--    sqlplus sys/Passw0rd! as sysdba
--    @oracle_test_setup.sql
-- 2) Le script crée ensuite les objets dans le schéma TEST et la procédure testable.
--
-- ATTENTION : mots de passe et users de test fournis uniquement pour DEV/TEST. Ne pas réutiliser en prod.

-- -----------------------------
-- 1) (Facultatif) supprimer l'utilisateur TEST s'il existe, puis le recréer
-- -----------------------------
BEGIN
  EXECUTE IMMEDIATE 'DROP USER TEST CASCADE';
EXCEPTION
  WHEN OTHERS THEN
    -- ignore si l'utilisateur n'existe pas
    NULL;
END;
/
-- Créer l'utilisateur TEST (exécuté en tant que SYSDBA)
BEGIN
  EXECUTE IMMEDIATE 'CREATE USER TEST IDENTIFIED BY "Passw0rd!"';
EXCEPTION
  WHEN OTHERS THEN
    NULL;
END;
/
-- Accorder privilèges au user TEST
BEGIN
  EXECUTE IMMEDIATE 'GRANT CREATE SESSION TO TEST';
  EXECUTE IMMEDIATE 'GRANT CREATE TABLE TO TEST';
  EXECUTE IMMEDIATE 'GRANT CREATE SEQUENCE TO TEST';
  EXECUTE IMMEDIATE 'GRANT CREATE PROCEDURE TO TEST';
  EXECUTE IMMEDIATE 'GRANT CREATE VIEW TO TEST';
  EXECUTE IMMEDIATE 'GRANT UNLIMITED TABLESPACE TO TEST';
EXCEPTION
  WHEN OTHERS THEN
    NULL;
END;
/
-- -----------------------------
-- 2) Se connecter en TEST et créer les objets de test
-- -----------------------------
CONNECT TEST/Passw0rd!
SET DEFINE OFF
-- Supprimer les objets s'ils existent (table/sequence/proc) - gestion via PL/SQL
BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE IDENTIFICATION_PROJET_TEST';
EXCEPTION
  WHEN OTHERS THEN
    NULL;
END;
/
BEGIN
  EXECUTE IMMEDIATE 'DROP SEQUENCE SEQ_IDENTIFICATION_PROJET';
EXCEPTION
  WHEN OTHERS THEN
    NULL;
END;
/
BEGIN
  EXECUTE IMMEDIATE 'DROP PROCEDURE AJOUTER_IDENTIFICATION_PROJET_ET_LISTES_JSON';
EXCEPTION
  WHEN OTHERS THEN
    NULL;
END;
/

COMMIT;

-- Créer table de test
CREATE TABLE IDENTIFICATION_PROJET_TEST (
  ID_IDENTIFICATION_PROJET VARCHAR2(50) PRIMARY KEY,
  NOM_PROJET               VARCHAR2(4000),
  JSON_SOURCE              CLOB,
  DATE_INSERTION           DATE DEFAULT SYSDATE
);

-- Créer sequence pour génération d'ID
CREATE SEQUENCE SEQ_IDENTIFICATION_PROJET START WITH 1 INCREMENT BY 1 NOCACHE;

-- Procédure de test : lit IdIdentificationProjet (si présent) ou génère via séquence, puis insère un enregistrement.
CREATE OR REPLACE PROCEDURE AJOUTER_IDENTIFICATION_PROJET_ET_LISTES_JSON(p_json IN CLOB) AS
  v_id_proj VARCHAR2(50);
  v_nom     VARCHAR2(4000);
BEGIN
  -- 1) Essayer la lecture du champ IdIdentificationProjet (camelCase)
  BEGIN
    v_id_proj := JSON_VALUE(p_json, '$.IdIdentificationProjet' RETURNING VARCHAR2);
  EXCEPTION
    WHEN OTHERS THEN
      v_id_proj := NULL;
  END;

  -- 2) Si absent, essayer la clé en MAJUSCULE (ID_IDENTIFICATION_PROJET)
  IF v_id_proj IS NULL THEN
    BEGIN
      v_id_proj := JSON_VALUE(p_json, '$.ID_IDENTIFICATION_PROJET' RETURNING VARCHAR2);
    EXCEPTION
      WHEN OTHERS THEN
        v_id_proj := NULL;
    END;
  END IF;

  -- 3) Si toujours absent, essayer dans l'objet Projets (Projets.IdIdentificationProjet)
  IF v_id_proj IS NULL THEN
    BEGIN
      v_id_proj := JSON_VALUE(p_json, '$.Projets.IdIdentificationProjet' RETURNING VARCHAR2);
    EXCEPTION
      WHEN OTHERS THEN
        v_id_proj := NULL;
    END;
  END IF;

  -- 4) Si toujours NULL, générer via la séquence
  IF v_id_proj IS NULL OR TRIM(v_id_proj) = '' THEN
    SELECT 'FQ' || TO_CHAR(SEQ_IDENTIFICATION_PROJET.NEXTVAL, 'FM000000') INTO v_id_proj FROM dual;
  END IF;

  -- 5) Tenter de lire NomProjet si présent
  BEGIN
    v_nom := JSON_VALUE(p_json, '$.NomProjet' RETURNING VARCHAR2);
  EXCEPTION
    WHEN OTHERS THEN
      v_nom := NULL;
  END;

  -- 6) Insert minimal dans la table de test
  INSERT INTO IDENTIFICATION_PROJET_TEST (ID_IDENTIFICATION_PROJET, NOM_PROJET, JSON_SOURCE)
  VALUES (v_id_proj, v_nom, p_json);

  COMMIT;
EXCEPTION
  WHEN OTHERS THEN
    -- En cas d'erreur, rollback et remonter l'erreur
    ROLLBACK;
    RAISE;
END AJOUTER_IDENTIFICATION_PROJET_ET_LISTES_JSON;
/

-- Optionnel : vérifier le nombre d'objets créés
PROMPT "Création terminée: table IDENTIFICATION_PROJET_TEST, sequence SEQ_IDENTIFICATION_PROJET, procedure AJOUTER_IDENTIFICATION_PROJET_ET_LISTES_JSON"

COMMIT;
EXIT;
