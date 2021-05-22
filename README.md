# UniversitySystem

UniversitySystem is ASP.NET core MVC application, which follows the 3 tiers architecture (`UI, Business and Data Access`). The web apllication supports students registration and login based on cookie authentication without identity. For storing password hashes is used `BCrypt.Net-Next` nuget package. After login the students can register to new course or can manage all courses (CRUD). Default Connection to SQL server is `.\\SQLEXPRESS`, it can be changed in `appsettings.json` file.
 
