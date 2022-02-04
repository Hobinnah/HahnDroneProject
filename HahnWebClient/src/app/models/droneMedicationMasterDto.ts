import { DecimalPipe } from "@angular/common";


export interface DroneMedicationMasterDto {

        droneMedicationMasterID: number;
        droneID: number;
        status: number;
        capturedBy: string;
        drone: any;
        droneMedicationDetails: any[];
}

