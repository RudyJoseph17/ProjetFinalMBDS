Param(
    [string]$OraclePassword = "Passw0rd!",
    [string]$ContainerName = "oracle-xe",
    [string]$ImageName = "gvenzl/oracle-xe:21.3.0-full-faststart",
    [string]$SqlFile = ".\oracle_test_setup.sql"
)

Write-Host "=== Étape 1: Lancement du conteneur Oracle XE ==="
docker run -d --name $ContainerName -p 1521:1521 -e ORACLE_PASSWORD=$OraclePassword $ImageName | Out-Null

Write-Host "=== Étape 2: Attente de la readiness du conteneur ==="
$ready = $false
for ($i = 0; $i -lt 30; $i++) {
    Start-Sleep -Seconds 10
    $logs = docker logs $ContainerName 2>&1
    if ($logs -match "DATABASE IS READY TO USE") {
        $ready = $true
        break
    }
    Write-Host "⏳ Attente... ($i/30)"
}

if (-not $ready) {
    Write-Error "❌ Oracle XE n'a pas démarré correctement. Vérifiez 'docker logs $ContainerName'."
    exit 1
}

Write-Host "=== Étape 3: Copie du script SQL dans le conteneur ==="
docker cp $SqlFile "${ContainerName}:/tmp/oracle_test_setup.sql"

Write-Host "=== Étape 4: Exécution du script SQL via sqlplus ==="
docker exec -i $ContainerName bash -c "source /home/oracle/.bashrc && sqlplus sys/$OraclePassword as sysdba @/tmp/oracle_test_setup.sql"


Write-Host "=== Étape 5: Définition de la variable d'environnement ORACLE_TEST_CONN pour la session PowerShell ==="
$env:ORACLE_TEST_CONN = "User Id=TEST;Password=$OraclePassword;Data Source=localhost:1521/XEPDB1"
Write-Host "✅ ORACLE_TEST_CONN défini : $env:ORACLE_TEST_CONN"

Write-Host "=== Fin du script: Oracle XE prêt pour les tests ==="
