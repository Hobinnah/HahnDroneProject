# HahnDroneAPP

### Introduction

This is a major new technology in the field of transportation: **the drone**. The drone has the potential to leapfrog traditional transportation infrastructure.

Useful drone functions include delivery of small items to not easily accessable locations.

---

### App description

We have a fleet of **drones**. A drone is capable of carrying and delivering small loads. For our use case **the loads are medications**.

A **Drone** has:
- serial number (100 characters max);
- model (Lightweight, Middleweight, Cruiserweight, Heavyweight);
- weight limit (500gr max);
- battery capacity (percentage);
- state (IDLE, LOADING, LOADED, DELIVERING, DELIVERED, RETURNING).

Each **Medication** has: 
- name (allowed only letters, numbers, ‘-‘, ‘_’);
- weight;
- code (allowed only upper case letters, underscore and numbers);
- image (picture of the medication case).


HahnDroneAPI is a .net core 3.1 platform independent web api application that is used in
1. registering a drone
2. loading a drone with medication items
3. checking loaded medication items for a given drone
4. checking available drones for loading
5. checking drone battery level for a given drone and 
6. runs a background process which keeps a log of all drones' battery level.

## Build
Use the command below to build the application from the project's root folder.
``` dotnet build ```

## Run

#### 1. Using Docker
   use the command below to run the application from the API's root folder.
   
   ``` docker-compose up -d```

   as soon as the command completes, go to the url below to see the API's swagger
   
   ```http://localhost:8080/swagger/index.html```

   and the below url to browse the client application

   ```http://localhost:4200```

#### 2. Visual Studio
   i. Using Visual Studio (API)
      a. start your visual studio application
      b. from visual studio, navigate to the API's root folder, find and select the file "HahnDroneAPI.sln"
      c. after visual studio loads the project, click on the run button or press F5 to run the application
      d. the app will load showing you a swagger page by default.

   ii. Using Visual Studio Code (Typescript)
      a. start your visual studio code application
      b. from visual studio code, navigate to the client application's folder, and select it ("HahnWebClient")
      c. after visual studio code loads the project, open a TERMINAL and type "ng serve --open" and click enter.
      d. after the command finishes, it will load the application on your browser.

#### 3. Command Prompt
   i. API
      a. Open your command prompt application
      b. Navigate to the project's root folder ~\HahnDroneSolution\HahnDroneAPI
      c. type "dotnet run" and click enter
      d. this will startup the application on port 5000 if available (localhost:{5000})
      e. to see the swagger page enter localhost:{5000}/swagger on your brower address tab

   ii. Client
      a. Ensure the API is running
      b. In the client application, find the file with name "proxy.conf.json", replace the value of the "target" key with the running API url
      c. Open the api.service.ts file under the services folder and uncomment the first this.basrUrl and comment the second one both inside the constructor.
      d. Open your command prompt application 
      e. Navigate to the project's root folder ~\HahnDroneSolution\HahnWebClient
      f. type "ng serve --open" and click enter
      g. this will startup the application on port 4200 if available, on your browser (localhost:{4200})


## Running the Unit Test
To run the API unit test, 
1. open the project with visual studio as decribed above
2. in solution explorer, select the test project (HahnDroneAPI.Test).
3. click the menu **Test** and then the sub-menu **Run All Tests**.

There are currently about Eighteen (18) test cases.

## Audit Event Log
This was implemented using hangfire. To see the log run 
```http://localhost:8080/api/AuditEventLog```
