# ExternalAuthentication

Steps

1. Change connection string from appsetting.json file i.e replace server name, user id and passsword
2. Open package manager console(Tools/Nuget Package Manager/Package Manager Console)
3. Run "update-database" command
4. Now get client id and client secret from google and facebook. Note: Redirect Uri must be https://localhost:5001/signin-google for google and https://localhost:5001/signin-facebook for facebook
5. Replace client id and client secret in appsetting.json
6. Run application click on signin
7. Choose method google or facebook
8. Thanks
