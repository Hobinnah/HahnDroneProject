import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { Alert, AlertEnum } from '../../models/models';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  private subjectAlert = new Subject<Alert>();
  alert: Alert;
  constructor() {

    this.alert = {
      AlertHeader: '',
      AlertType: '',
      AlertMessage: '',
      ShowAlert: false
    };

  }

  sendAlertMessage(objectMessage: Alert) {

    this.subjectAlert.next(objectMessage);

  }

  clearAlertMessages() {

    this.subjectAlert.next();

  }

  getAlertMessage(): Observable<Alert> {

    return this.subjectAlert.asObservable();

  }

  ShowSuccessAlert(message: string, timeout: number = 100000): any {

    this.alert.AlertType = AlertEnum.SUCCESS;
    this.alert.AlertHeader = AlertEnum.SUCCESShEADER;
    this.alert.AlertMessage = message;
    this.alert.ShowAlert = true;

    setTimeout(() => {
      this.alert.ShowAlert = false;
      return this.alert;
    }, timeout);

    return this.alert;
  }

  ShowDangerAlert(message: string, timeout: number = 25000): any {

    this.alert.AlertType = AlertEnum.DANGER;
    this.alert.AlertHeader = AlertEnum.DANGERhEADER;
    this.alert.AlertMessage = message;
    this.alert.ShowAlert = true;

    setTimeout(() => {
        this.alert.ShowAlert = false;
        return this.alert;
      }, timeout);

    return this.alert;
  }

  

}
