
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgbPaginationModule, NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { AgmCoreModule } from '@agm/core';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AppbodyComponent } from './appbody/appbody.component';
import { AppbreadcrumComponent } from './shared/appbreadcrum/appbreadcrum.component';
import { AppheaderComponent } from './shared/appheader/appheader.component';


import { SideMenuComponent } from './shared/side-menu/side-menu.component';
import { MobileMenuComponent } from './shared/mobile-menu/mobile-menu.component';


import { StorageService } from './services/storageService/storage.service';
import { ApiService } from './services/apiService/api.service';
import { DroneService } from './services/droneService/drone-service.service';
import { MessageService } from './services/messageService/message.service';
import { ExcelService } from './services/excel-service/excel.service';

import { AlertComponent } from './shared/alert/alert.component';
import { DronesComponent } from './drones/drones.component';
import { DataService }from './services/dataService/datah-service.service';
import { LoadDroneComponent } from './loadDrone/load-drone.component';
import { DroneMapComponent } from './droneMap/drone-map.component';


@NgModule({
  declarations: [
    AppComponent,
    AppbodyComponent,
    AppbreadcrumComponent,
    AppheaderComponent,
    SideMenuComponent,
    MobileMenuComponent,
    AlertComponent,
    DronesComponent,
    LoadDroneComponent,
    DroneMapComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    NgxUiLoaderModule,
    NgbPaginationModule,
    NgbAlertModule,
    AgmCoreModule.forRoot({
      apiKey: 'AIzaSyB4bGCRkmXnUub9mUZ3xZvSQvxU3-6SVQY'
    }),
    
  ],
  providers: [
    StorageService,
    ApiService,
    MessageService,
    ExcelService,
    DroneService,
    DataService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
