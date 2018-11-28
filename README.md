# Planning Poker
Exam project for the Analysis, Design and Software Architecture course at ITU

### Getting the system up and running
This guide makes the following assumptions:
* You have a running MSSQL server, and that appSettings.Development.json has a matching connectionString.
* You have .NET Core installed

Do the following:
* Run `dotnet restore` in the root of the solution (you can skip this, if you are using an IDE. Most of them does this for you).
* Go into the `PlanningPoker.Entities` project.
* Run `dotnet ef database update -s ../PlanningPoker.WebApi`.
* Run `dotnet run` to start the API.

#### Run MSSQL via Docker
Assuming that you have Docker and docker-compose installed, run the following command from the solutions root directory:  
`docker-compose up`  

If you wish to nuke your database and start fresh, run the following:  
`docker-compose down`  
`docker-compose up`

When in doubt, just ask Elias <3.
