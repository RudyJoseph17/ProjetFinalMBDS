```markdown

\# Architecture de la solution \*InvestissementsPublics.sln\*



La solution est organisée en plusieurs \*\*modules\*\* et \*\*couches\*\* pour séparer responsabilités et faciliter la maintenance.



---



\## Structure globale



```plaintext

InvestissementsPublics.sln

│

├── Starter

│   └── InvestissementsPublics.Starter (MVC + Auth)

│

├── BanqueProjet

│   ├── BanqueProjet.Web

│   ├── BanqueProjet.API

│   ├── BanqueProjet.Application

│   └── BanqueProjet.Infrastructure

│

├── Programmation

│   ├── Programmation.Web

│   ├── Programmation.API

│   ├── Programmation.Application

│   └── Programmation.Infrastructure

│

├── SuiviEvaluation

│   ├── SuiviEvaluation.Web

│   ├── SuiviEvaluation.API

│   ├── SuiviEvaluation.Application

│   └── SuiviEvaluation.Infrastructure

│

├── TableauxDeBord

│   ├── TableauxDeBord.Web

│   ├── TableauxDeBord.API

│   ├── TableauxDeBord.Application

│   └── TableauxDeBord.Infrastructure

│

└── Shared
	├── Shared.Domain (entités communes, auth, rôles)

	└── Shared.Infrastructure (entités communes, auth, rôles)

&nbsp;   


## Description des dossiers

- **BanqueProjet.Web** : interface utilisateur MVC (ajout, mise à jour et suppression d'un projet)
- **BanqueProjet.API** : Web API REST  
- **BanqueProjet.Application** : logique métier et DTOs  
- **BanqueProjet.Infrastructure** : accès aux données via EF Core  

- **Programmation.Web** : interface utilisateur MVC (ajout, mise à jour et suppression d'une programmation annuelle)  
- **Programmation.API** : Web API REST  
- **Programmation.Application** : logique métier et DTOs  
- **Programmation.Infrastructure** : accès aux données via EF Core 

- **SuiviEvaluation.Web** : interface utilisateur MVC (ajout, mise à jour et suppression de bilan annuel, de releve de depense)  
- **SuiviEvaluation.API** : Web API REST  
- **SuiviEvaluation.Application** : logique métier et DTOs  
- **SuiviEvaluation.Infrastructure** : accès aux données via EF Core 

- **SuiviEvaluation.Web** : interface utilisateur MVC pour les tableaux de bord dynamique
- **SuiviEvaluation.API** : Web API REST  
- **SuiviEvaluation.Application** : logique métier et DTOs  
- **SuiviEvaluation.Infrastructure** : accès aux données via EF Core 

- **Shared** : composants et entités partagés (authentification, paramètres)


