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
- JWT
- OpenID Connect (OIDC)
- Object List To CSV Download
- Import CSV to Object List

