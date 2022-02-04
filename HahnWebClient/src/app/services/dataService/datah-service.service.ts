import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ModelDto } from '../../models/modelDto';
import { StateEnum } from '../../enums/StateEnum';
import { ApiService } from '../apiService/api.service';
import { MedicationDto } from 'src/app/models/medicationDto';
import { ExcelService } from './../excel-service/excel.service';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  private exportData: any[];
  private stateData: any[];
  private base = 'Model';

  constructor(private apiService: ApiService, private excelService: ExcelService) {

      this.exportData = [];
   }

  getModels(): Observable<ModelDto[]> {

      const url = this.base;
      return this.apiService.getAll(url, this.apiService.getHttpHeadersAnonymous());
  }

  getMedications(): Observable<MedicationDto[]> {

    const url = "Medication";
    return this.apiService.getAll(url, this.apiService.getHttpHeadersAnonymous());
}

  getDroneStates(): any[] {

      this.stateData = [];
      let len = Object.keys(StateEnum).length / 2;
      for(let i=0; i < len; i++) {

         this.stateData.push({state: i, description: StateEnum[i].toString()});
      }

      return this.stateData;
  }
  
  async filterMedication(filterBy: string, droneMeds: MedicationDto[]) {

    filterBy = filterBy.toLocaleLowerCase();
    return droneMeds.filter((med: MedicationDto) => med.name.toLowerCase().indexOf(filterBy) !== -1 || med.code.toLowerCase().indexOf(filterBy) !== -1 || med.weight === parseFloat(filterBy));
  }

  onExportAsXLSX(droneMedications: MedicationDto[]) {

    this.exportData = [];
    let data: any = {};

    if (droneMedications) {

        for (const med of droneMedications) {

          data.droneID = med.medicationID;
          data.batteryCapacity = med.name;
          data.serialNumber = med.code;
          data.weight = med.weight;
          
          this.exportData.push(data);
          data = {};
        }

        this.excelService.exportAsExcelFile(this.exportData, 'DroneMedications');
    }
}

}
