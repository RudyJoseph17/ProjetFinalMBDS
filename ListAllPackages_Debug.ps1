<#
    Debug script : affiche les projets détectés, la sortie brute de dotnet list,
    et remonte toute erreur de parsing JSON.
#>

# Passe dans le dossier du script
Set-Location $PSScriptRoot
Write-Host "Working directory:" (Get-Location) -ForegroundColor Cyan

# Récupération des .csproj
$csprojs = Get-ChildItem -Recurse -Filter *.csproj
Write-Host "Projets trouvés :" $csprojs.Count

if ($csprojs.Count -eq 0) {
    Write-Host "⚠️  Aucune cible .csproj détectée" -ForegroundColor Yellow
    exit 1
}

$collected = @()

foreach ($proj in $csprojs) {
    $path = $proj.FullName
    Write-Host "`n▶️  Traitement de : $path" -ForegroundColor White

    # Appel dotnet list avec capture de toute la sortie
    $raw = dotnet list $path package --include-transitive --format json 2>&1
    Write-Host "  → dotnet list exit code: $LASTEXITCODE"
    Write-Host "  → raw output (premières lignes) :"
    $raw | Select-Object -First 5 | ForEach-Object { Write-Host "    $_" }

    if ($LASTEXITCODE -ne 0) {
        Write-Host "  ✖ dotnet list a échoué pour ce projet" -ForegroundColor Red
        continue
    }

    try {
        $json = $raw | ConvertFrom-Json
    }
    catch {
        Write-Host "  ✖ Impossible de parser le JSON :" $_.Exception.Message -ForegroundColor Red
        continue
    }

    foreach ($p in $json.projects) {
        foreach ($fx in $p.frameworks.GetEnumerator()) {
            foreach ($pkg in $fx.Value.packages) {
                $collected += [PSCustomObject]@{
                    Project     = $proj.BaseName
                    Framework   = $fx.Key
                    PackageName = $pkg.name
                    Requested   = $pkg.requestedVersion
                    Resolved    = $pkg.resolvedVersion
                    Type        = $pkg.type
                }
                Write-Host "     • $($pkg.name) $($pkg.resolvedVersion)" -ForegroundColor Green
            }
        }
    }
}

if ($collected.Count -gt 0) {
    $out = Join-Path $PSScriptRoot 'packages.csv'
    $collected | Sort-Object Project,PackageName | Export-Csv -Path $out -NoTypeInformation
    Write-Host "`n✔ Exporté $($collected.Count) packages dans $out" -ForegroundColor Cyan
}
else {
    Write-Host "`nℹ️  Aucun package collecté." -ForegroundColor Yellow
}
