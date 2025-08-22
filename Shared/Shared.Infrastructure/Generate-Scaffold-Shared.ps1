

param (
    [string]$tables,
    [string]$mappingFile
)

# 1. Charger .env
$envPath = Join-Path $PSScriptRoot '.env'
if (!(Test-Path $envPath)) {
    Write-Error ".env introuvable à $envPath"
    exit 1
}
Get-Content $envPath | ForEach-Object {
    if ($_ -match "^\s*([^#][^=]+)\s*=\s*(.*)\s*$") {
        [System.Environment]::SetEnvironmentVariable($matches[1], $matches[2])
    }
}

# 2. Récupérer variables Oracle
$User     = $env:ORACLE_DB_USER
$Password = $env:ORACLE_DB_PASSWORD
$DbHost   = $env:ORACLE_DB_HOST
$Port     = $env:ORACLE_DB_PORT
$Service  = $env:ORACLE_DB_SERVICE

if (-not ($User -and $Password -and $DbHost -and $Port -and $Service)) {
    Write-Error "Une ou plusieurs variables Oracle manquent dans .env"
    exit 1
}

# 3. Construire chaîne de connexion
$connectionString = "User Id=$User;Password=$Password;Data Source=$DbHost`:$Port/$Service;"
Write-Host "🛠️  Connexion Oracle : $connectionString" -ForegroundColor Yellow

# 4. Déterminer chemins des projets
$infraProj = Join-Path $PSScriptRoot '..\Shared.Infrastructure\Shared.Infrastructure.csproj'
$apiProj   = Join-Path $PSScriptRoot '..\Shared.API\Shared.API.csproj'

if (!(Test-Path $infraProj)) { Write-Error "Shared introuvable : $infraProj"; exit 1 }
if (!(Test-Path $apiProj))   { Write-Error "Shared introuvable : $apiProj"; exit 1 }

# 5. Préparer liste des tables
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
        Write-Error "Impossible de lire mappingFile, vérifiez le JSON"; exit 1
    }
} else {
    Write-Error "Vous devez fournir -tables ou -mappingFile pour spécifier les vues à scaffolder."; exit 1
}

# 6. Scaffolder chaque table
foreach ($tbl in $tableList) {
    Write-Host "⚙️  Scaffold de la table/vue : $tbl" -ForegroundColor Cyan
    $efParams = @(
        $connectionString,
        'Oracle.EntityFrameworkCore',
        '--project', (Resolve-Path $infraProj).Path,
        '--startup-project', (Resolve-Path $apiProj).Path,
        '--context-dir', 'Data',
        '--output-dir', "..\shared.Infrastructure\Entities",
        '--context', 'SharedDbContext',
        '--table', $tbl,
        '--data-annotations',
        '--force'
    )
    dotnet ef dbcontext scaffold @efParams
}
