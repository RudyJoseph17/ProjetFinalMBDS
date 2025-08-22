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

&nbsp;   └── Shared.Domain (entités communes, auth, rôles)

