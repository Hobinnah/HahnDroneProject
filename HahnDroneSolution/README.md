# HahnDroneAPI

HahnDroneAPI is a .net core 3.1 platform independent web api application that is used in
1. registering a drome
2. loading a drone with medication items
3. checking loaded medication items for a given drone
4. checking available drones for loading
5. checking drone battery level for a given drone and 
6. runs a background process which keeps a log of all drones' battery level.

## Build
Use the command below to build the application from the project's root folder.
``` dotnet build ```

## Run

1. Using Docker
   use the command below to run the application from the root folder.
   
   ``` docker-compose up -d```

   as soon as the command completes, go to the url below
   
   ```http://localhost:8080/swagger/index.html```

2. Using Visual Studio
   a. start your visual studio application
   b. from visual studio, navigate to the application's root folder, find and select the file "HahnDroneAPI.sln"
   c. after visual studio loads the project, click on the run button or press F5 to run the application
   d. the app will load showing you a swagger page by default.

3. Command Prompt
   a. Open your command prompt application
   b. Navigate to the project's root folder ~\HahnDroneSolution\HahnDroneAPI
   c. type "dotnet run" and click enter
   d. this will startup the application on port 5000 if available (localhost:{5000})
   e. to see the swagger page enter localhost:{5000}/swagger on your brower address tab


## Running the Unit Test
To run the unit test, 
1. open the project with visual studio as decribed above
2. in solution explorer, select the test project (HahnDroneAPI.Test).
3. click the menu **Test** and then the sub-menu **Run All Tests**.

There are currently about twenty-one (25) test cases.

## Audit Event Log
This was implemented using hangfire. To see the log run 
```http://localhost:8080/api/AuditEventLog```
