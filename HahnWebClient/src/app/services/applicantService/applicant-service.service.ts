import { ExcelService } from './../excel-service/excel.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IApplicant } from '../../models/applicants';
import { ApiService } from '../apiService/api.service';

@Injectable({
  providedIn: 'root'
})
export class ApplicantService {

  private exportData: any[];
  private base = 'Applicant';
  applicants: IApplicant[];

  constructor(private apiService: ApiService, private excelService: ExcelService) {

   }

  getApplicants(): Observable<IApplicant[]> {

      const url = this.base;
      return this.apiService.getAll(url, this.apiService.getHttpHeadersAnonymous());
  }

  getApplicant(applicantID: number): Observable<IApplicant> {

      const url = `${this.base}/${applicantID}`;
      return this.apiService.get(url, this.apiService.getHttpHeadersAnonymous());
  }

  createApplicant(applicant: IApplicant): Observable<IApplicant> {

      const url = `${this.base}`;
      return this.apiService.post(applicant, url, this.apiService.getHttpHeadersAnonymous());
  }

  updateApplicant(applicantID: number, applicant: IApplicant): Observable<IApplicant> {

      const url = `${this.base}/${applicantID}`;
      return this.apiService.update(url, this.apiService.getHttpHeadersAnonymous(), applicant);
  }

  deleteApplicant(applicantID: number): Observable<{}> {

      const url = `${this.base}/${applicantID}`;
      return this.apiService.delete(url, this.apiService.getHttpHeadersAnonymous());
  }

  async filterApplicant(filterBy: string, applicants: IApplicant[]) {

      filterBy = filterBy.toLocaleLowerCase();
      return applicants.filter((applicant: IApplicant) => applicant.countryOfOrigin.toLowerCase().indexOf(filterBy) !== -1 ||
                                                          applicant.familyName.toLowerCase().indexOf(filterBy) !== -1 ||
                                                          applicant.name.toLowerCase().indexOf(filterBy) !== -1);
  }

  validateCountry(country): Observable<any> {

    const url = `${this.base}/ValidateCountry/${country}`;
    return this.apiService.get(url, this.apiService.getHttpHeadersAnonymous());
  }

  onExportAsXLSX(applicants: IApplicant[]) {

      this.exportData = [];
      let data: any = {};

      if (applicants) {

          for (const applicant of applicants) {

            data.applicantID = applicant.id;
            data.name = applicant.name;
            data.familyName = applicant.familyName;
            data.age = applicant.age;
            data.address = applicant.address;
            data.countryOfOrigin = applicant.countryOfOrigin;
            data.eMailAdress = applicant.eMailAddress;
            data.hired = applicant.hired;

            this.exportData.push(data);
            data = {};
          }

          this.excelService.exportAsExcelFile(this.exportData, 'Applicants');
      }
  }

}
