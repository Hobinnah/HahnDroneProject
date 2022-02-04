import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';



@Component({
  selector: 'app-mobile-menu',
  templateUrl: './mobile-menu.component.html',
  styleUrls: ['./mobile-menu.component.css']
})


export class MobileMenuComponent implements OnInit, OnDestroy {

  globalUser: any;
  subscription: Subscription;

  username = 'Eze Obinna J';
  role = 'Software Engineer';
  constructor() {


  }

  ngOnInit() {
    this.initializeMenu();
  }

  ngOnDestroy() {
    // unsubscribe to ensure no memory leaks
    if (this.subscription) {
        this.subscription.unsubscribe();
    }
  }

  initializeMenu() {



  }

  lockApp() {

    this.logout();

  }

   logout() {



   }
}
