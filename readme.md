# 'Remedium' web application
As of now, this application consists of:
* Landing page, where authorized users may add news articles
* Forum page, where all registered users may exchange messages

## Technologies used
* ASP NET5 MVC
* Entity Framework Core
* Bootstrap 5
* Popper.js
* SQLite
* AutoMapper

## Instructions
* Make sure that .NET 5 Sdk is installed (dotnet --version)
* Download the repository as .zip file
* In directory of the solution execute command dotnet build
* In directory of the project (Remedium.Web) execute command dotnet run
* In your preferred browser input the url shown in the console window (https://localhost:5000 or https://localhost:5000 by default)
* Alternatively open the .sln file in your IDE
* This application uses SQLite, therefore running migrations is not required

## Default user accounts
* (administrator & moderator role assigned) login: admin@test.net; password: admin1
* (moderator role assigned) login: moderator@test.net; password: moderator
* login: gilgamesh; password: gilgamesh
* login: cyrus; password: cyrus1
* login: temujin; password: temujin
