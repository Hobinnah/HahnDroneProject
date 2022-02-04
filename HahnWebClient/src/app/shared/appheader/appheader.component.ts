import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-appheader',
  templateUrl: './appheader.component.html',
  styleUrls: ['./appheader.component.css']
})
export class AppheaderComponent implements OnInit {

  roles: any;
  administrator = false;

  username = 'Default';
  role = 'Default';
  constructor() {

  }

   ngOnInit() {}

   logout() {


   }

   globalPolicySearch(value) {


  }

}
