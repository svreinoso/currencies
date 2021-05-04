# currencies

- .NET Core 5.0 with Entity Framework

- Angular 11

- Basic implementation of UnitTest

### Setup

- Clone the repository 

- Setup the Sql Server connection string in Currencies/appSettings.json 

- Can run with visual studio or dotnet cli

### Answers

It is definitely not safe to pass the id as input of the endpoint, I consider that it should be taken from the user who has logged in, for example from the token or the object of the session and validate that the user is active and other things that will be defined by the business logic. 

### Notes

This is a simple implementation using SPA that includes the backend and frontend in the same project, but it is divided by layers that makes it scalable, manageable and easy to maintain.

For the database, Sql Server is used with the Code first pattern, when the project is run for the first time it must create the database and run the migrations, if the connection string has not been configured correctly it will throw an exception
 