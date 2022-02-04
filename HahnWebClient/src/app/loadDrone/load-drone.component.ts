import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Alert, BreadCrumb, DroneDto, MedicationDto, ModelDto, StateEnum, DroneMedicationRequest } from '../models/models';
import { MessageService, StorageService, DroneService, DataService } from '../services/services';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-load-drone',
  templateUrl: './load-drone.component.html',
  styleUrls: ['./load-drone.component.css']
})
export class LoadDroneComponent implements OnInit {


  breadcrumb: Array<BreadCrumb>;
  alert: Alert;
  exportData: any[];

  drone: DroneDto;
  droneID: number;
  request: DroneMedicationRequest;

  memList: MedicationDto[] = [];
  medications: MedicationDto[] = [];
  medicationList: MedicationDto[] = [];
  droneMedicationList: MedicationDto[] = [];
  medicationMemoryList: MedicationDto[] = [];

  pageSize = 10;
  page = 1;
  memPage = 1;

  droneMedForm: FormGroup;
  submitted = false;
  isViewRecord = false;

  constructor(private droneService: DroneService, private dataService: DataService, private storageService: StorageService, private formBuilder: FormBuilder,
              private loader: NgxUiLoaderService, private messageService: MessageService, private activatedRoute: ActivatedRoute, private router: Router) {

      // The Pages Breadcrumb
      this.breadcrumb = [
          { Text: 'Home',  Link: '/home', Class: '', Params: null},
          { Text: 'Drones',  Link: '/drones', Class: 'active', Params: null},
          { Text: 'Loading Drone',  Link: '/loaddrone', Class: 'active', Params: { 'id': this.droneID } }

      ];

      // drone Initialization
      this.initializeDrone();
      this.droneID = 0;
      this.medicationMemoryList = [];
      
      this.activatedRoute.params.subscribe(
        params => this.droneID = parseInt(params['id'])
      );

      this.storageService.saveBreadCrumb(this.breadcrumb);

  }

  initializeDrone() {

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

    this.medications = [];

    this.loader.start();
    this.getDrone();
    this.droneMedications(this.droneID);
    this.getAllMedications();
    this.initializeForm();
  }

  getDrone() {

      this.droneService.getDrone(this.droneID).subscribe(data => {

          const result: any = data;
          if (data) {
             
             this.drone = result;
             this.onEditDrone(this.drone);
          } else {
            this.initializeDrone();
          }

          this.loader.stop();
          // console.log(data);
        },
        err => {  this.loader.stop(); });

  }

  getAllMedications() {

    this.dataService.getMedications().subscribe(data => {

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

  }

  droneMedications(droneID) {

    this.loader.start();
    this.medications = [];
    this.droneService.getDroneMedications(droneID).subscribe(data => {

      const result: any = data;
      if (data) {
        this.droneMedicationList = result.medications;
        this.memList = this.droneMedicationList;
      } else {
        this.droneMedicationList = [];
      }

      this.loader.stop();
      //console.log(data);
    },
    err => {  this.loader.stop(); });

    return false;
  }
  
  initializeForm() {

    this.droneMedForm = this.formBuilder.group({
          SerialNumber: new FormControl('', [Validators.required, Validators.maxLength(100)])
    });

  }

  onSubmit() {

      this.submitted = true;

      // stop here if form is invalid
      if (this.droneMedForm.invalid || this.medicationMemoryList.length === 0) {
           return;
      }

      this.loader.start();
      if (this.droneMedForm.value.SerialNumber !== '') {

              this.request = {
                droneID: 0,
                medicationIDs: []
              };

              this.request.droneID = this.drone.droneID;
              let medIDs : number[] = [];

              for(const id of this.medicationMemoryList){
                 
                 medIDs.push(id.medicationID);
              }

              this.request.medicationIDs = medIDs;

              console.log('request', this.request);

              // Send new drone data to server.
              this.droneService.loadDroneWithMedication(this.request).subscribe(data => {
                  
                  if (data) {

                        this.droneMedForm.reset();
                        this.droneMedications(this.drone.droneID);
                        this.medicationMemoryList = [];
                        // Shows Alert Message ...
                        this.alert = this.messageService.ShowSuccessAlert('Drone was successfully loaded.');
                        this.messageService.sendAlertMessage(this.alert);
                        this.loader.stop();
                  } else {

                    this.loader.stop();
                    // Shows Alert Message ...
                    this.alert = this.messageService.ShowDangerAlert('An error occurred. Please try again later;');
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

  onMap() {

    this.router.navigateByUrl('droneMap');
    return false;
  }
 
  async filterMedications(value) {

    this.droneMedicationList = await this.dataService.filterMedication(value, this.memList);
  }

  addDrone() {

      // Resets the form.
      this.droneMedForm.reset();
      this.submitted = false;
      this.isViewRecord = false;
  }

  onEditDrone(drone: DroneDto) {

      if (drone) {

        this.isViewRecord = false;

        this.droneMedForm.reset();
        this.droneMedForm.patchValue({
          SerialNumber: drone.serialNumber
        });

      }

      return false;
  }
  
  onMemoryAddMedication(medicationID) {

    if (!medicationID) {
      return false;
    }

    let total = this.totalMedWeigth();
    const mem = this.medications.filter((item) => (item.medicationID === parseInt(medicationID, 0)));
    if (!mem || (total + mem[0].weight) > this.drone.weight) {
        
       this.alert = this.messageService.ShowDangerAlert(`Adding this medication, will cause the total medication's weight (${ total + mem[0].weight }milligram), to exceed the allowed weight for this drone, which is ${ this.drone.weight }milligram`);
       this.messageService.sendAlertMessage(this.alert);
       return false;
    }

    if (mem && this.medicationMemoryList.indexOf(mem[0]) === -1) {
       this.medicationMemoryList.push(mem[0]);

       // Remove the med that has already been selected.
       this.removeDuplicateFromMemory();
    }

    return false;

  }

  removeDuplicateFromMemory() {

      // Remove the med that has already been selected.
      for (const r of this.droneMedicationList) {
        for (const t of this.medicationMemoryList) {
            if (r.medicationID === t.medicationID) {
               const k = this.medicationMemoryList.indexOf(t);
               this.medicationMemoryList.splice(k, 1);
            }
        }
     }

  }

  onMemoryDeleteMedication(medication) {

    let i = -1;

    i = this.medicationMemoryList.indexOf(medication);
    this.medicationMemoryList.splice(i, 1);
    
    return false;
  }

  totalMedWeigth() {
      
     let total = 0;

     if (this.droneMedicationList) {
        for (const r of this.droneMedicationList) {
            total += r.weight;
        }
    }

    if (this.medicationMemoryList) {
        for (const t of this.medicationMemoryList) {
            total += t.weight;
        }
    }

     return total;

  }

  onExportAsXLSX() {

    this.loader.start();
    if (this.droneMedicationList) {

        this.dataService.onExportAsXLSX(this.droneMedicationList);
    }

    this.loader.stop();
    return false;
  }

}
