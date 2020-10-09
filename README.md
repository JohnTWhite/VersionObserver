# VersionObserver
Queries Azure Devops Repository for CSProject references. Saves references and version to database.

- Recursively Search File Directory for CSProj files.
- Recursively Query Azure Dev Ops repositories through Azure Api for CSProj files.
- Save project references, and versions, to dedicated database.

## Search File Directory
- Performance wise this is the most performant. 
- The directory search is platform agnostic. This aids in contanrization, so that a container built in a linux (Debian, Alpine) environment will perform exactly as it would in windows. 
- Once built, the first argument passed to the application should be the directory the user would like to be recursively searched. Then the application will find all contained CSProj files and save project refrences, and version to the dedicated database.

## Query Azure Dev Ops API
- Create a PAT (Personal Access Token) in Azure Devops.
- Configure azure devops token VersionObserver.json in configs folder. Ensure you also provide a cookie.
- Run the application without any command line arguments.
- This will recursively search all repositories for CSProj files, convert them to type XMLDocument, and save project refrences, and version to the dedicated database.
