import { Params } from '@angular/router';

export interface BreadCrumb {

   Link: string;
   Params?: Params;
   Text: string;
   Class: string;
}
