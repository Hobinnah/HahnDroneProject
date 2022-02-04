
import { ExcelService } from './../excel-service/excel.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DroneDto } from '../../models/droneDto';
import { MedicationDto } from '../../models/medicationDto';
import { StateEnum } from '../../enums/StateEnum';
import { ApiService } from '../apiService/api.service';

@Injectable({
  providedIn: 'root'
})
export class DroneService {

  private exportData: any[];
  private base = 'Drone';
  drones: DroneDto[];

  constructor(private apiService: ApiService, private excelService: ExcelService) {

   }

  getDrones(): Observable<DroneDto[]> {

      const url = this.base;
      return this.apiService.getAll(url, this.apiService.getHttpHeadersAnonymous());
  }

  getDrone(droneID: number): Observable<DroneDto> {

      const url = `${this.base}/GetDrone/${droneID}`;
      return this.apiService.get(url, this.apiService.getHttpHeadersAnonymous());
  }

  getDroneMedications(droneID: number): Observable<any> {

    const url = `${this.base}/${droneID}/Medications`;
    return this.apiService.get(url, this.apiService.getHttpHeadersAnonymous());
}

  createDrone(drone: DroneDto): Observable<DroneDto> {

      const url = `${this.base}/Create`;
      return this.apiService.post(drone, url, this.apiService.getHttpHeadersAnonymous());
  }

  updateDrone(droneID: number, drone: DroneDto): Observable<DroneDto> {

      const url = `${this.base}/Update/${droneID}`;
      return this.apiService.update(url, this.apiService.getHttpHeadersAnonymous(), drone);
  }

  deleteDrone(droneID: number): Observable<{}> {

      const url = `${this.base}/Delete/${droneID}`;
      return this.apiService.delete(url, this.apiService.getHttpHeadersAnonymous());
  }

  loadDroneWithMedication(droneMed: any): Observable<any> {

      const url = `${this.base}/LoadDroneWithMedication`;
      return this.apiService.post(droneMed, url, this.apiService.getHttpHeadersAnonymous());
  }

  async filterDrone(filterBy: string, drones: DroneDto[]) {

      filterBy = filterBy.toLocaleLowerCase();
      return drones.filter((drone: DroneDto) => drone.serialNumber.toLowerCase().indexOf(filterBy) !== -1 || drone.stateDescription.toLowerCase().indexOf(filterBy) !== -1 || drone.model.description.toLowerCase().indexOf(filterBy) !== -1 || drone.batteryCapacity === parseFloat(filterBy));
  }

  onExportAsXLSX(drones: DroneDto[]) {

      this.exportData = [];
      let data: any = {};

      if (drones) {

          for (const drone of drones) {

            data.droneID = drone.droneID;
            data.serialNumber = drone.serialNumber;
            data.batteryCapacity = drone.batteryCapacity;
            data.model = drone.model.description;
            data.state = StateEnum[drone.state];
            data.weight = drone.weight;
            
            this.exportData.push(data);
            data = {};
          }

          this.excelService.exportAsExcelFile(this.exportData, 'Drones');
      }
  }

}
