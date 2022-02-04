
import { Injectable, ErrorHandler, Injector } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Alert } from '../models/models';
import { MessageService } from '../services/services';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    alert: Alert;
    constructor(private messageService: MessageService, private router: Router) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        return next.handle(request).pipe(catchError(err => {

            this.errorManager(err);
            // location.reload(true);

            const error = err.error.description || err.statusText;
            return throwError(error);
        }));
    }

    errorManager(err) {
      let theErrorMessage = '';

      if (err.status === 400) {
            // theErrorMessage = 'Please check your input values, some fields where not supplied properly. ';
            theErrorMessage = err.error.description || err.error.title;
      } else if (err.status === 401) {
            this.router.navigate(['/shared/error401']);

      } else if (err.status === 403) {
            this.router.navigate(['/shared/error403']);

      } else if (err.status === 500) {
        theErrorMessage = 'Internal Error, please contact the system administrator. ';
        this.router.navigate(['/shared/error500']);

      } else if (err.status === 504) {
        theErrorMessage = 'Please contact the system administrator, as the application service is currently unavaliable. ';
      }

      this.alert = this.messageService.ShowDangerAlert(`${theErrorMessage} ${err.statusText}`);
      this.messageService.sendAlertMessage(this.alert);

    }
}
