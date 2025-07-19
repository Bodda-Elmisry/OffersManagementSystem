Architecture :-
	Clean Code
Projects :
	- Domain :- Responce for entities and configrations
		Refrances :
			- Microsoft.EntityFrameworkCore, Version : 8.0.18
	
	- Application :- Responce for contructes
		Refrances :
			- Domain Project
	
	- Infrastructure :- Responce for Migrations, Implementations and Data
		Refrances :
			- Application Project
			- Dapper, Version: 2.1.66
			- Microsoft.AspNetCore.Identity.EntityFrameworkCore, Version: 8.0.18
	        - Microsoft.EntityFrameworkCore.SqlServer, Version: 8.0.18
            - Microsoft.EntityFrameworkCore.Tools, Version: 8.0.18
			
	- Web :- The presentation Layer
		Refrances :
			- Infrastructure Project
			- Mapster, Version: 7.4.0
			- Microsoft.AspNetCore.Authentication.JwtBearer, Version: 8.0.18
	        - Microsoft.EntityFrameworkCore.Design, Version: 8.0.18
		
			
			
			
			
Instructions :-
 1- Change connection string in "appsettings.json"
 2- open "Package Manager Console"
 3- Select "OffersManagementSystem.Infrastructure" from "Default Project" dropdown
 4- Run command "Update-Database"
 5- Set "OffersManagementSystem.Web" as startup Project
 6- Run Project
