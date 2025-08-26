param (
    [string]$tables,
    [string]$mappingFile
)

# 1. Charger .env
$envPath = Join-Path $PSScriptRoot '.env'
if (!(Test-Path $envPath)) {
    Write-Error ".env introuvable a $envPath"
    exit 1
}
Get-Content $envPath | ForEach-Object {
    if ($_ -match "^\s*([^#][^=]+)\s*=\s*(.*)\s*$") {
        [System.Environment]::SetEnvironmentVariable($matches[1], $matches[2])
    }
}

# 2. Recuperer variables Oracle
$User     = $env:ORACLE_DB_USER
$Password = $env:ORACLE_DB_PASSWORD
$DbHost   = $env:ORACLE_DB_HOST
$Port     = $env:ORACLE_DB_PORT
$Service  = $env:ORACLE_DB_SERVICE
$Schema   = $env:ORACLE_DB_SCHEMA   # <-- Nouveau

if (-not ($User -and $Password -and $DbHost -and $Port -and $Service)) {
    Write-Error "Variables Oracle manquantes dans .env"
    exit 1
}

# 3. Construire chaine de connexion (info pour logs uniquement)
$connectionString = "User Id=$User;Password=****;Data Source=$DbHost`:$Port/$Service;"
Write-Host "[INFO] Connexion Oracle (masquee) : $connectionString" -ForegroundColor Yellow
if ($Schema) {
    Write-Host "[INFO] Schema cible : $Schema" -ForegroundColor Yellow
}

# 4. Determiner chemins des projets
$infraProj = Join-Path $PSScriptRoot '..\Shared.Infrastructure\Shared.Infrastructure.csproj'
$apiProj   = Join-Path $PSScriptRoot '..\Shared.API\Shared.API.csproj'

if (!(Test-Path $infraProj)) { Write-Error "Projet Infrastructure introuvable : $infraProj"; exit 1 }
if (!(Test-Path $apiProj))   { Write-Error "Projet API introuvable : $apiProj"; exit 1 }

# 5. Preparer liste des tables
$tableList = @()
if ($tables) {
    $tableList = $tables.Split(',') | ForEach-Object { $_.Trim() } | Where-Object { $_ }
} elseif ($mappingFile) {
    if (!(Test-Path $mappingFile)) {
        Write-Error "Fichier mapping introuvable : $mappingFile"; exit 1
    }
    try {
        $json = Get-Content $mappingFile -Raw | ConvertFrom-Json
        $tableList = $json.Tables
    } catch {
        Write-Error "Impossible de lire mappingFile, verifiez le JSON"; exit 1
    }
} else {
    Write-Error "Vous devez fournir -tables ou -mappingFile pour specifier les vues a scaffolder."
    exit 1
}

if ($tableList.Count -eq 0) {
    Write-Error "Aucune table/vue trouvee a scaffolder."
    exit 1
}

# 6. Construire les parametres EF Core
$efParams = @(
    "User Id=$User;Password=$Password;Data Source=$DbHost`:$Port/$Service;",
    'Oracle.EntityFrameworkCore',
    '--project', (Resolve-Path $infraProj).Path,
    '--startup-project', (Resolve-Path $apiProj).Path,
    '--context-dir', 'Data',
    '--output-dir', "..\Shared.Infrastructure\Entities",
    '--context', 'SharedDbContext',
    '--data-annotations',
    '--no-onconfiguring',   # <-- Ajout clé ici
    '--force'
)

# Ajouter schema si defini
if ($Schema) {
    $efParams += @('--schema', $Schema)
}

# Ajouter toutes les tables
foreach ($tbl in $tableList) {
    Write-Host "[TABLE] Ajout de la vue/table : $tbl" -ForegroundColor Cyan
    $efParams += @('--table', $tbl)
}

# 7. Executer le scaffolding une seule fois
Write-Host "[SCAFFOLD] Generation des entites EF Core pour $($tableList.Count) vues..." -ForegroundColor Green
dotnet ef dbcontext scaffold @efParams
