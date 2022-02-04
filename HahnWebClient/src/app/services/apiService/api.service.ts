import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  public errorMessage = '';
  private baseUrl = '';
  constructor(private http: HttpClient) {

        //this.baseUrl = '/api/'; 
        this.baseUrl = 'http://localhost:8080/api/';
  }

  private handleError<T>(err: HttpErrorResponse) {

    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      errorMessage = `Server returned code : ${err.status}, error message is : ${err.message}`;
    } else {
      errorMessage = `${err.error.description}`;
    }

    console.log(errorMessage);

    return throwError(errorMessage);
  }


  getHttpHeadersAnonymous(): HttpHeaders {
    const headers = new HttpHeaders().set('content-type', 'application/json')
                                     .set('Access-Control-Allow-Origin', '*')
                                     .set('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');

    return headers;
  }

  get<T>(apiUrl, httpHeader: HttpHeaders): Observable<T> {
    const url = `${this.baseUrl}${apiUrl}`;
    return this.http.get<T>(url, { headers: httpHeader, withCredentials: true }).pipe(
                     tap(e => console.log('All' + JSON.stringify(e))),
                     catchError(this.handleError)
    );
  }

  getAll<T>(apiUrl, httpHeader: HttpHeaders): Observable<T[]> {

    console.log(apiUrl);
    return this.http.get<T[]>(`${this.baseUrl}${apiUrl}`, { headers: httpHeader }).pipe(
            tap(e => console.log('All' + JSON.stringify(e))),
            catchError(this.handleError)
      );
  }

  post<T>(data, apiUrl, httpHeader: HttpHeaders): Observable<T> {
      const url = `${this.baseUrl}${apiUrl}`;
      console.log(url);
      return this.http.post<T>(url, data, { headers: httpHeader, withCredentials: true  }).pipe(
                       tap(e => console.log('All' + JSON.stringify(e))),
                       catchError(this.handleError)
      );
  }

  update<T>(apiUrl, httpHeader: HttpHeaders, data): Observable<T> {
      const url = `${this.baseUrl}${apiUrl}`;
      return this.http.put<T>(url, data, { headers: httpHeader, withCredentials: true  }).pipe(
                      tap(e => console.log('All' + JSON.stringify(e))),
                      catchError(this.handleError)
      );
  }

  delete<T>(apiUrl, httpHeader: HttpHeaders): Observable<T> {
      const url = `${this.baseUrl}${apiUrl}`;
      return this.http.delete<T>(url, { headers: httpHeader, withCredentials: true  }).pipe(
                       tap(e => console.log('All' + JSON.stringify(e))),
                       catchError(this.handleError)
      );
  }

}
