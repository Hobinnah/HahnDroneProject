import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Alert, BreadCrumb, DroneDto, MedicationDto, ModelDto, StateEnum } from '../models/models';
import { MessageService, StorageService, DroneService, DataService } from '../services/services';
import { FormGroup, FormBuilder, FormControl, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { NgxUiLoaderService } from 'ngx-ui-loader';



function numberRange(min: number, max: number): ValidatorFn {
  return (c: AbstractControl): {[key: string]: boolean} | null => {
      if (c.value !== null && !isNaN(c.value) && (c.value >= min && c.value <= max)) {
          return null;
      }

      return { 'range': false };
  };
}


@Component({
  selector: 'app-drones',
  templateUrl: './drones.component.html',
  styleUrls: ['./drones.component.css']
})
export class DronesComponent implements OnInit {

  breadcrumb: Array<BreadCrumb>;
  alert: Alert;

  drones: DroneDto[];
  droneList: DroneDto[];
  drone: DroneDto;

  models: ModelDto[];
  states: any[];
  medications: MedicationDto[];

  pageSize = 10;
  page = 1;

  droneForm: FormGroup;
  submitted = false;
  isViewRecord = false;

  constructor(private droneService: DroneService, private dataService: DataService, private storageService: StorageService, private formBuilder: FormBuilder,
              private loader: NgxUiLoaderService, private messageService: MessageService, private route: Router) {

      // The Pages Breadcrumb
      this.breadcrumb = [
          { Text: 'Home',  Link: '/home', Class: '', Params: null},
          { Text: 'Drones',  Link: '/drones', Class: 'active', Params: null}

      ];

      this.storageService.saveBreadCrumb(this.breadcrumb);

      // drone Initialization
      this.drone = {
        droneID: 0,
        modelID: 0,
        serialNumber: '',
        weight: 0,
        state: 0,
        stateDescription: '',
        batteryCapacity: 0,
        model: {}
      };

  }

  ngOnInit() {

    this.drones = [];
    this.models = [];
    this.states = [];
    this.medications = [];

    this.loader.start();
    this.getAllDrones();
    this.getAllModels();
    this.getAllDroneStates();
    this.initializeForm();
  }

  getAllDrones() {

      this.droneService.getDrones().subscribe(data => {

          const result: any = data;
          if (data) {
             this.getDroneStateDescriptions(data)
             this.drones = result.drones;
             this.droneList = this.drones;
          } else {
             this.drones = [];
          }

          this.loader.stop();
          // console.log(data);
        },
        err => {  this.loader.stop(); });

  }

  getAllModels() {

    this.dataService.getModels().subscribe(data => {

        const result: any = data;
        if (data) {
           this.models = result.models;
        } else {
           this.models = [];
        }

        this.loader.stop();
        // console.log(data);
      },
      err => {  this.loader.stop(); });

}

getAllDroneStates() {

      const data = this.dataService.getDroneStates();
      if (data) {
         this.states = data;
      } else {
         this.states = [];
      }   

}

  //getStateDescription(state) { return StateEnum[state]; }
  getDroneStateDescriptions(data) {

     for(let i=0; i < data.count; i++) {
        
        data.drones[i].stateDescription = StateEnum[data.drones[i].state];
     }
  }
  
  initializeForm() {

    this.droneForm = this.formBuilder.group({
          DroneID: new FormControl(''),
          ModelID: new FormControl('', [Validators.required]),
          SerialNumber: new FormControl('', [Validators.required, Validators.maxLength(100)]),
          Weight: new FormControl('', [Validators.required, numberRange(1, 500)]),
          State: new FormControl('', [Validators.required]),
          BatteryCapacity: new FormControl('', [Validators.required, numberRange(1, 100)])
    });

  }

  onSubmit() {

      this.submitted = true;

      // stop here if form is invalid
      if (this.droneForm.invalid) {
           return;
      }

      this.loader.start();
      if (this.droneForm.value.DroneID === '' || this.droneForm.value.DroneID === 0 || this.droneForm.value.DroneID === null) {

              this.drone.droneID = 0;
              this.drone.batteryCapacity = parseInt(this.droneForm.value.BatteryCapacity, 0);
              this.drone.modelID = parseInt(this.droneForm.value.ModelID, 0);
              this.drone.serialNumber = this.droneForm.value.SerialNumber;
              this.drone.state = parseInt(this.droneForm.value.State, 0);
              this.drone.weight = parseFloat(this.droneForm.value.Weight);
              
              console.log('drone', this.drone);

              // Send new drone data to server.
              this.droneService.createDrone(this.drone).subscribe(data => {
                  if (data) {

                        this.getAllDrones();
                        this.droneForm.reset();

                        // Shows Alert Message ...
                        this.alert = this.messageService.ShowSuccessAlert('Drone was successfully saved.');
                        this.messageService.sendAlertMessage(this.alert);
                        this.loader.stop();
                  } else {

                    this.loader.stop();
                    // Shows Alert Message ...
                    this.alert = this.messageService.ShowDangerAlert('An error occurred. Please provide valid values;');
                    this.messageService.sendAlertMessage(this.alert);
                  }
                  this.submitted = false;
                },
                err => {
                  this.loader.stop();
                  this.alert = this.messageService.ShowDangerAlert(`An error occurred. ${err}`);
                  this.messageService.sendAlertMessage(this.alert);
              });
      } else {
            // Send new drone data to server.
              this.drone.droneID = parseInt(this.droneForm.value.DroneID, 0);
              this.drone.batteryCapacity = parseInt(this.droneForm.value.BatteryCapacity, 0);
              this.drone.modelID = parseInt(this.droneForm.value.ModelID, 0);
              this.drone.serialNumber = this.droneForm.value.SerialNumber;
              this.drone.state = parseInt(this.droneForm.value.State, 0);
              this.drone.weight = parseFloat(this.droneForm.value.Weight);

                  // Update drone ...
              this.droneService.updateDrone(this.drone.droneID, this.drone).subscribe(data => {
                if (data) {
                      this.getAllDrones();
                      this.droneForm.reset();

                      // Shows Alert Message ...
                      this.alert = this.messageService.ShowSuccessAlert('Drone was successfully updated.');
                      this.messageService.sendAlertMessage(this.alert);

                      this.loader.stop();
                } else {

                  this.loader.stop();
                  //   // Shows Alert Message ...
                  this.alert = this.messageService.ShowDangerAlert('An error occurred. Please provide valid values');
                  this.messageService.sendAlertMessage(this.alert);
                }
                this.submitted = false;
              },
              err => {
                  this.loader.stop();
                  this.alert = this.messageService.ShowDangerAlert(`An error occurred. ${err}`);
                  this.messageService.sendAlertMessage(this.alert);
              });
      }

      return false;
  }

  viewMedications(droneID) {

      this.loader.start();
      this.medications = [];
      this.droneService.getDroneMedications(droneID).subscribe(data => {

        const result: any = data;
        if (data) {
          this.medications = result.medications;
        } else {
          this.medications = [];
        }

        this.loader.stop();
        console.log(data);
      },
      err => {  this.loader.stop(); });

      return false;
  }

  onLoad(droneID){


    this.route.navigateByUrl(`loaddrone/${droneID}`);
    return false;
  }


  async filterDrones(value) {

    this.drones = await this.droneService.filterDrone(value, this.droneList);
  }

  addDrone() {

      // Resets the form.
      this.droneForm.reset();
      this.submitted = false;
      this.isViewRecord = false;
  }

  onEditDrone(drone: DroneDto) {

      this.drone = {
        droneID: 0,
        modelID: 0,
        serialNumber: '',
        weight: 0,
        state: 0,
        stateDescription: '',
        batteryCapacity: 0,
        model: {}
      };


      if (drone) {

        this.isViewRecord = false;

        this.droneForm.reset();
        this.droneForm.patchValue({
          DroneID: drone.droneID,
          ModelID: drone.modelID,
          SerialNumber: drone.serialNumber,
          Weight: drone.weight,
          State: drone.state,
          BatteryCapacity: drone.batteryCapacity
        });

      }

      return false;
  }

  onViewDrone(drone: any) {

      this.onEditDrone(drone);
      this.isViewRecord = true;

      return false;
  }

  onDeleteDrone(drone: DroneDto) {

      if (drone.droneID) {

        if (drone.state > 0) {

            this.alert = this.messageService.ShowDangerAlert(`The drone with serial number ${drone.serialNumber} can't be deleted.`);
            this.messageService.sendAlertMessage(this.alert);
            return false;
        }

        if (confirm('Are you sure you want to delete the drone with serial number ' + drone.serialNumber + '?')) {
              
          this.loader.start();
          this.droneService.deleteDrone(drone.droneID).subscribe(data => {
                if (data) {
                      this.getAllDrones();

                      // Shows Alert Message ...
                      this.alert = this.messageService.ShowSuccessAlert('Drone was successfully deleted.');
                      this.messageService.sendAlertMessage(this.alert);
                }
                this.loader.stop();
              },
              err => { this.loader.stop(); });
        } else {
           this.loader.stop();
        }
      }

      return false;
  }

  resetForm() {

      if (confirm('Are you sure you want to clear all data on the form?')) {

          this.droneForm.reset();
      }

      return false;
  }

  onExportAsXLSX() {

    this.loader.start();
    if (this.drones) {

        this.droneService.onExportAsXLSX(this.drones);
    }

    this.loader.stop();
    return false;
  }

}
