## Server first time config

Create a **.env** file, and then:

`dotnet restore`

`dotnet tool install --global dotnet-ef --version 9.0.3` then _follow recommended zsh steps to fix path_

`dotnet ef migrations add InitialCreate`

`dotnet ef database update`

`dotnet build`

`dotnet run`
