import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { StorageService } from '../../services/storageService/storage.service';
import { BreadCrumb } from '../../models/breadCrumb';

@Component({
  selector: 'app-appbreadcrum',
  templateUrl: './appbreadcrum.component.html',
  styleUrls: ['./appbreadcrum.component.css']
})
export class AppbreadcrumComponent implements OnInit {

  breadCrumb: Array<BreadCrumb>;
  constructor(private storageService: StorageService, private router: Router) {

    router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {

        // Loads the breadcrumb...
          this.loadBreadCrumb();
      }
    });
  }

  ngOnInit() {
  }

  loadBreadCrumb() {

    const url = this.router.url.toString().toLowerCase().split('/');
    this.breadCrumb = this.storageService.getBreadCrumb();
    if (this.breadCrumb === null || this.breadCrumb.length === 0) {

      this.breadCrumb = [
        { Text: 'Home',  Link: '/pages/home', Class: '', Params: null},
        { Text: 'Drones',  Link: '/drones', Class: 'active', Params: null}
      ];

    } else {
        this.breadCrumb = this.storageService.getBreadCrumb();
    }
  }

}
