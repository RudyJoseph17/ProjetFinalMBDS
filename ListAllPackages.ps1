<#
.SYNOPSIS
    Liste tous les packages NuGet (directs + transitifs) de chaque projet .csproj en .NET 8.

.DESCRIPTION
    - Parcourt tous les .csproj sous le dossier du script.
    - Appelle `dotnet list package --include-transitive --format json`.
    - Pour chaque framework : extrait topLevelPackages et transitivePackages.
    - Exporte un CSV avec : Project, Framework, PackageName, Requested, Resolved, PackageType.
#>

Set-Location $PSScriptRoot

$csprojs = Get-ChildItem -Recurse -Filter *.csproj
if ($csprojs.Count -eq 0) {
    Write-Host "⚠️  Aucun .csproj trouvé." -ForegroundColor Yellow
    exit 1
}

$results = @()

foreach ($file in $csprojs) {
    $projName = $file.BaseName
    Write-Host "`n▶ Traitement de $projName" -ForegroundColor Cyan

    $raw = dotnet list $file.FullName package --include-transitive --format json 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "  ✖ Échec dotnet list $projName" -ForegroundColor Red
        continue
    }

    try {
        $data = $raw | ConvertFrom-Json
    }
    catch {
        Write-Host "  ✖ JSON invalide pour $projName :" $_.Exception.Message -ForegroundColor Red
        continue
    }

    foreach ($proj in $data.projects) {
        # .NET 8 : frameworks est un tableau d'objets { framework; topLevelPackages; transitivePackages }
        $frameworks = $proj.frameworks

        foreach ($fx in $frameworks) {
            $fxName = $fx.framework
            # Références directes
            foreach ($pkg in $fx.topLevelPackages) {
                $results += [PSCustomObject]@{
                    Project     = $projName
                    Framework   = $fxName
                    PackageName = $pkg.id
                    Requested   = $pkg.requestedVersion
                    Resolved    = $pkg.resolvedVersion
                    PackageType = 'Direct'
                }
                Write-Host "   • Direct : $($pkg.id) -> $($pkg.resolvedVersion)" -ForegroundColor Green
            }
            # Références transitives
            foreach ($pkg in $fx.transitivePackages) {
                $results += [PSCustomObject]@{
                    Project     = $projName
                    Framework   = $fxName
                    PackageName = $pkg.id
                    Requested   = ''
                    Resolved    = $pkg.resolvedVersion
                    PackageType = 'Transitive'
                }
                Write-Host "   • Transitive : $($pkg.id) -> $($pkg.resolvedVersion)" -ForegroundColor DarkGray
            }
        }
    }
}

if ($results.Count -gt 0) {
    $csvPath = Join-Path $PSScriptRoot 'packages.csv'
    $results | Sort-Object Project,Framework,PackageName |
        Export-Csv -Path $csvPath -NoTypeInformation
    Write-Host "`n✔ Export terminé : $csvPath" -ForegroundColor Cyan
}
else {
    Write-Host "`nℹ️  Aucun package collecté." -ForegroundColor Yellow
}
