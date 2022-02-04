import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  saveBreadCrumb(breadCrumb) {
    localStorage.setItem('applicantBreadCrumb', JSON.stringify(breadCrumb) );
  }

  getBreadCrumb(): any {
    const data = localStorage.getItem('applicantBreadCrumb');

    if (data !== undefined && data !== null) {
       return JSON.parse(data);
    } else {
        return [
          { Text: 'Home',  Link: '/pages/home'},
          { Text: 'Applicants',  Link: '/Applicants'}
      ];
    }
  }

  removeBreadCrumb() {
    localStorage.removeItem('applicantBreadCrumb');
  }

}
