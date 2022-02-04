
import { StateEnum } from "../enums/StateEnum";


export interface DroneDto {
        droneID: number;
        modelID: number;
        serialNumber: string;
        weight: number;
        batteryCapacity: number;
        state: StateEnum;
        stateDescription: string;
        model: any;
}

