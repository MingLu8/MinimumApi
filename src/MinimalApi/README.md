# Code Samples for MinimumApi
- Route Setup *
- Swagger *
- Global Exception Handling *
- Development Exception handling is automatically turned on if the environment name is `Development` and using `WebApplication.CreateBuilder` 
- Problem Details *
- File Uploads *
- File Downloads *
- Cors *
- Authentication *
- Authorization *
  - Authentication and Authorization need to be added after Cors, 
  - For testing purpose the sample uses user-jwts, to create a token use: `dotnet user-jwts create --scope "greetings_api" --role "admin"`
  - then use `curl -i -H "Authorization: Bearer {token}" https://localhost:{port}/customers` to test the endpoint, remember to replace the token and port.
- JWT *
- Swagger JWT Authorization Integration *
- Endpoint grouping *
- Http Editor, .http files *
- OpenID Connect (OIDC)
- Object List To CSV Download
- Import CSV to Object List
- Unit Tests
- Integraton Tests
- Use Record type for Database object
- Use Dapper or RepoDb
- GraphQL
- Add docker file
- Add CI/CD GitHub Action Integration 
- Add Azure Appplication Insight Integration
- Parameter validation
- Add Code Coverage


#Notes
Run SQL-Server in Windows Docker Container
- `docker pull mcr.microsoft.com/mssql/server:2019-latest`
- `docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=34r0TNhvgOde" -p 1433:1433 --name sql2019 -h sql2019 -d mcr.microsoft.com/mssql/server:2019-latest`
- - connect to the sql server instance on docker as normal, 
  ![Connect To Sql On Docker](../../docs/connect-to-sql-on-docker.png)