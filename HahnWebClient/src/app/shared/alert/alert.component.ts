import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Alert } from '../../models/models';
import { MessageService } from '../../services/services';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})
export class AlertComponent implements OnInit, OnDestroy {

  alert: Alert;
  subscriptionAlert: Subscription;

  constructor(private messageService: MessageService) {

    // Alert Initialization
    this.alert = {
      AlertHeader: '',
      AlertType: '',
      AlertMessage: '',
      ShowAlert: false
    };

    this.initializeAlert();

  }

  ngOnInit() {

  }

  ngOnDestroy() {
    // unsubscribe to ensure no memory leaks
    if (this.subscriptionAlert) {
        this.subscriptionAlert.unsubscribe();
    }
  }

 initializeAlert() {

  this.subscriptionAlert = this.messageService.getAlertMessage().subscribe(message => {

    console.log('alert : ' + message);
    if (message) {
      this.alert = message;
    }

  });

 }


}
