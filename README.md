# MVC / MySQL / Identity / Role Manager
A quick start on the road to great software free from Microsoft licenses.

## Getting Started

### Install the template

From working/templates/identity-mysql$ run ```dotnet new --install ./``` or .\ on windows.

### Create your project

dotnet new freemvc -n myfoldername

### Customise and profit

First, add your MySQL connection string to the appsettings.json and check your MySQL Server version, 
update Program.cs to make it match.

Next apply the migration to your database by using the dotnet-ef tool:
```dotnet ef database update```

Once you have registered, visit the role manager and add the Administrator role.
Add yourself to the Administrator role and then remove the commented protection of the role controller.
