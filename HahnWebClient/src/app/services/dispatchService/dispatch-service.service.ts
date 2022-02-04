
import { ExcelService } from './../excel-service/excel.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DroneDto } from '../../models/droneDto';
import { ApiService } from '../apiService/api.service';

@Injectable({
  providedIn: 'root'
})
export class DispatchService {

  private exportData: any[];
  private base = 'Dispatch';
  drones: DroneDto[];

  constructor(private apiService: ApiService, private excelService: ExcelService) {

   }

  getdrones(): Observable<DroneDto[]> {

      const url = this.base;
      return this.apiService.getAll(url, this.apiService.getHttpHeadersAnonymous());
  }

  getdrone(droneID: number): Observable<DroneDto> {

      const url = `${this.base}/${droneID}`;
      return this.apiService.get(url, this.apiService.getHttpHeadersAnonymous());
  }

  createdrone(drone: DroneDto): Observable<DroneDto> {

      const url = `${this.base}`;
      return this.apiService.post(drone, url, this.apiService.getHttpHeadersAnonymous());
  }

  updatedrone(droneID: number, drone: DroneDto): Observable<DroneDto> {

      const url = `${this.base}/${droneID}`;
      return this.apiService.update(url, this.apiService.getHttpHeadersAnonymous(), drone);
  }

  deletedrone(droneID: number): Observable<{}> {

      const url = `${this.base}/${droneID}`;
      return this.apiService.delete(url, this.apiService.getHttpHeadersAnonymous());
  }

  async filterdrone(filterBy: string, drones: DroneDto[]) {

      filterBy = filterBy.toLocaleLowerCase();
      return drones.filter((drone: DroneDto) => drone.serialNumber.toLowerCase().indexOf(filterBy) !== -1 || drone.batteryCapacity === parseFloat(filterBy));
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
            data.state = drone.state;
            data.weight = drone.weight;
            
            this.exportData.push(data);
            data = {};
          }

          this.excelService.exportAsExcelFile(this.exportData, 'Drones');
      }
  }

}
