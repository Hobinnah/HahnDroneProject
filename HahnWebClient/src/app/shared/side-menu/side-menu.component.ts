import { Component, OnInit } from '@angular/core';
// import { AuthenticationService, StorageService, MessageService, LoaderService } from '../../services/services';


@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.css']
})
export class SideMenuComponent implements OnInit {

  globalUser: any;
  permissions: Array<{any}>;
  roles: any[];

  administrator = false;
  policyApproval = false;
  adminApproval = false;
  policyMasters = false;

  username = 'Eze Obinna J';
  role = 'Software Engineer';
  constructor() {


  }

  ngOnInit() {

    // this.userPermission(); // remove
    // this.initializeMenu();
  }

  initializeMenu() {


  }

  userPermission() {



  }

  lockApp() {

    this.logout();

  }

   logout() {



   }
}
