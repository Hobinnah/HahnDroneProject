import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppbodyComponent } from './appbody/appbody.component';
import { DronesComponent } from './drones/drones.component';
import { LoadDroneComponent } from './loadDrone/load-drone.component';
import { DroneMapComponent } from './droneMap/drone-map.component';

const routes: Routes = [


  { path: '', component: AppbodyComponent,
    children: [
            { path: '', component: DronesComponent },
            { path: 'drones', component: DronesComponent},
            { path: 'loaddrone/:id', component: LoadDroneComponent},
            { path: 'droneMap', component: DroneMapComponent},
    ]
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
