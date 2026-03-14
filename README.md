# DeveloperAssessment

## Dependencies

To run the application and view the database you will need to install:

* .NET 8.0 SDK : https://dotnet.microsoft.com/en-us/download/dotnet/8.0
* DB Browser (or equivalent SQLite database viewer) : https://sqlitebrowser.org/dl/

## Database and CSV location

When the DatabaseSetup console application is run, it will create the database file `mydatabase.db` (if it doesn't already exist) in one of the following locations depending on your OS:

* Windows: `C:\Users\<username>\AppData\Local\mydatabase.db`
* MacOS: `/Users/<username>/Library/Application Support/mydatabase.db` (in Finder, Go → Go to Folder ~/Library/Application Support)
* Linux: `/home/<username>/.local/share/mydatabase.db`

Similarly, the ETLApp application will create orders.csv in the same folder.

In case either file cannot be found on your system, I have included sample output files in the repository.

## Data flow and how-to

Before starting, navigate to the DeveloperAssessment directory from the command line and run `dotnet build`. Please keep the command line open in this directory for the next steps.

### Setting up the database 

The database setup script is found in the DatabaseSetup project. This is a C# console application that will:

1. Create a SQLite database in the user's LocalApplicationData directory.
1. Create the required tables.
1. Load the sample dataset from dataset.json.
1. Insert the sample Customers and Orders.

To run the script, enter the command `dotnet run --project DatabaseSetup`.

If desired, the database file can then be opened in DB Browser and the data queried.

### Running the API

The API project consists of NET Minimal API. To run this, enter the command `dotnet run --project API`

Then, open a browser window and enter the localhost URL provided by the console (e.g. http://localhost:5000/).

The API exposes the following endpoint:

`GET /customers/{id}`

Example:

http://localhost:5000/customers/1

This endpoint returns:

* Customer details
* All associated orders for that customer

If the customer does not exist, a 404 Not Found response is returned.

### Running the ETL pipeline

The database setup script is found in the ETLApp project. This is a C# console application that will:

1. Query the database for active customers and their orders
1. Transform the data
1. Export the results to a CSV file

To run the script, enter the command `dotnet run --project ETLApp`.

This application could easily be run on a schedule by publishing to an executable file, and then using the Windows Task Scheduler (for example) to run it.

## Choices and reasoning

I chose to build the assignment using C# as I am currently a .NET developer and it is my strongest language. The assignment allowed any language or framework, so I used technologies I could implement confidently in the time given.

Other choices I made included:

### Dataset
I created the dataset in a separate JSON file, as JSON maps cleanly to C# models using .NET's JSON serialiser.

### Database

I chose SQLite because it is lightweight and requires no server installation, which greatly simplifies running the assignment locally.

### API

I built the API as an ASP.NET Core Minimal API, as it works well with the .NET ecosystem and allowed me to define an endpoint very concisely.

### ORM (Object-Relational Mapper)

I installed the Entity Framework Core library as an Object-Relational Mapper, removing the need to write SQL directly and greatly simplifying database interaction in general. It also allowed me to re-use the same database context class and model classes across projects, as I wrote a C# class library called Data and then referenced this in all three applications.

## What I would improve

I deliberately kept the solution very lightweight due to time constraints and for ease of understanding. However, given more time or in a production scenario, I would:

1. Improve error handling by using specific exception types, adding a greater number of defensive checks, and installing a logging framework
1. Replace the SQLite database with a more robust SQL Server database hosted on a separate DB server
1. Move the database connection string into a separate configuration file
1. Implement a data access layer with repository pattern for database interactions
1. Implement a service layer for business logic
1. Implement unit and integration tests
1. Implement security on the API endpoint, such as an API key