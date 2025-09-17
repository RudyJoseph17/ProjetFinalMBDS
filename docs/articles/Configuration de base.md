# Configuration de base

## Prérequis

- .NET 8 SDK  
- SQL Oracle 
- Visual Studio 2022 ou VS Code

## Configuration des connexions

Utilisation de fichier .env avec la Configuration suivante:

# Nom du compte Oracle (user) utilisé pour se connecter à la base
ORACLE_DB_USER=<oracle_username>

# Mot de passe du compte Oracle
ORACLE_DB_PASSWORD=<oracle_password>

# Adresse (hostname ou IP) du serveur Oracle
ORACLE_DB_HOST=<db_host>

# Port d’écoute du listener Oracle (généralement 1521)
ORACLE_DB_PORT=<db_port>

# Nom du service (SERVICE_NAME) ou SID de la base Oracle
ORACLE_DB_SERVICE=<service_name>

