# Q-Nomy

Q-Nomy home assigment

עליך ליצור עמוד אינטרנט שבו כל משתמש יכול להכניס לתור, לקרוא לממתין ולצפות ברשימת הממתינים.
העמוד צריך להיראות באופן הבא:

![example view](docs/view01.png)

כל הרשומות ישמרו בDB לפי סטטוס הפניה הנוכחי שלהם:
סטטוס ממתין: 0
סטטוס בשירות: 1
סטטוס הושלם טיפול: 2
* 	הקריאה להבא בתור תהיה בתצורת FIFO.
* 	לא תתאפשר הכנסה אנונימית לתור.
* 	מבנה הטבלאות והנתונים נתון לשיקול דעתך.
3 הגישה לבסיס הנתונים תיעשה דרך web service/Web API.


### Prerequisites

1. [Angular 9+](http://angular.io/)
2. [.NET Core 3.1](https://dotnet.microsoft.com/download)
3. [MsSQL Server Express 2017 Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

Everything used with default settings without additional installation. Just simple Next, Next, Next should be fine

### Installing

1. Clone this git repostiory: `git clone https://github.com/jasper22/Q-Nomy.git`
2. Enter into Q-Nomy\src\client and install all Angular packages: `npm install`
3. Restore all dotnet core packages by running: `dotnet restore QNomy.sln` in root folder of git repostiory
4. Review SQL connection string at: `Q-Nomy\src\server\appsettings.json`. Currently it points to default location of default installation of SQL Server Express. Make changes if you need
5. If you don't have Microsoft Entity Core 3.1 Tools installed you can install it in Powershell by running command: `Install-Package Microsoft.EntityFrameworkCore.Tools` from Administrative prompt
	* In Visual Studio 2019 it's already pre-installed and you could run those command from 'Package Manager' window
	* More information is here: [Entity Framework Core tools reference - Package Manager Console in Visual Studio](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell)
6. Update/Install database by running: `Update-Database` 
7. Once all this completes sucsesfully you are ready to run this client/server application


### Running
1. Enter into `Q-Nomy\src\server` folder and run the server by command: `dotnet run`. Server should start and listening on http://localhost on port 5000 
2. Enter into `Q-Nomy\src\client` folder and start the client app by running `ng serve`. Once client is started navigate your browser to: [http://localhost:4200/](http://localhost:4200/)
