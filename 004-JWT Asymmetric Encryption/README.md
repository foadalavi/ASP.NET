open development console and go to the AuthServr.API folder
```cmd
cd .\AuthServer.Api\
```

make sure you have entity framework installed on your machine. Depending on if you installed it globally or not it should be listed using one of these two commands:
```cmd
dotnet tool list -g
dotnet tool list
```

Or you can easily try:
```cmd
dotnet ef
```
If you see the EF unicorn it is installed and ready to use otherwise you need to install it using:
```cmd
dotnet tool install dotnet-ef -g
```
Create the migration using the following command:
```cmd
dotnet ef migrations add initial
```
And then update/create the database using this command:
```cmd
dotnet ef database update
```

Now you are ready to run this application.