# Architecture

Explication de l’architecture du projet :

## Modules principaux

- **BanqueProjet** : Web, API, Application, Infrastructure
- **Programmation** : Web, API, Application, Infrastructure
- **SuiviEvaluation** : Web, API, Application, Infrastructure
- **TableauxDeBord** : Web, API, Application, Infrastructure
- **Shared.Domain** : entités communes, authentification, rôles

## Flux

- Starter → API modules
- API → Couche Métier (Application)
- Application → Infrastructure et Shared.Domain


## Diagramme des flux (Mermaid)

```mermaid
flowchart TB

subgraph UI [Interface Utilisateur]
    Starter[Starter (Auth MVC)]
    BanqueWeb[BanqueProjet.Web]
    ProgWeb[Programmation.Web]
    SuiviWeb[SuiviEvaluation.Web]
   
end

subgraph API [APIs REST]
    BanqueAPI[BanqueProjet.API]
    ProgAPI[Programmation.API]
    SuiviAPI[SuiviEvaluation.API]
   
end

subgraph Application [Couches Métier]
    BanqueApp[BanqueProjet.Application]
    ProgApp[Programmation.Application]
    SuiviApp[SuiviEvaluation.Application]
    DashApp[TableauxDeBord.Application]
end

subgraph Infrastructure [Accès Données]
    BanqueInfra[BanqueProjet.Infrastructure]
    ProgInfra[Programmation.Infrastructure]
    SuiviInfra[SuiviEvaluation.Infrastructure]
    
end

subgraph Shared [Partagé]
    SharedDomain[Shared.Domain]
end

Starter --> BanqueAPI
Starter --> ProgAPI
Starter --> SuiviAPI

BanqueWeb --> BanqueAPI
ProgWeb --> ProgAPI
SuiviWeb --> SuiviAPI

BanqueAPI --> BanqueApp
ProgAPI --> ProgApp
SuiviAPI --> SuiviApp

BanqueApp --> BanqueInfra
ProgApp --> ProgInfra
SuiviApp --> SuiviInfra

BanqueApp --> SharedDomain
ProgApp --> SharedDomain
SuiviApp --> SharedDomain
DashApp --> SharedDomain