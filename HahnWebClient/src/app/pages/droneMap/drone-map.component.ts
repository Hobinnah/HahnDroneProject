import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Alert, BreadCrumb, DroneDto } from '../../models/models';
import { MessageService, StorageService, DroneService } from '../../services/services';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
  selector: 'app-drone-map',
  templateUrl: './drone-map.component.html',
  styleUrls: ['./drone-map.component.css']
})
export class DroneMapComponent implements OnInit {

  breadcrumb: Array<BreadCrumb>;
  alert: Alert;

  drones: DroneDto[];

  title = 'Angular Google Maps Example';

  lat = 13;
  lng = 80;

  constructor(private droneService: DroneService, private storageService: StorageService, 
    private loader: NgxUiLoaderService, private messageService: MessageService, private route: Router) {

      // The Pages Breadcrumb
      this.breadcrumb = [
      { Text: 'Home',  Link: '/home', Class: '', Params: null},
      { Text: 'Drones',  Link: '/drones', Class: 'active', Params: null},
      { Text: 'Map',  Link: '/droneMap', Class: 'active', Params: null}

      ];

      this.storageService.saveBreadCrumb(this.breadcrumb);


  }

  ngOnInit() {}

}
