import { NgModule } from '@angular/core';
import { ThemeModule } from '../../@theme/theme.module';
import { ToasterModule } from 'angular2-toaster';
import { Ng2SmartTableModule, LocalDataSource } from 'ng2-smart-table';
import { WarehouseRoutingModule, routedComponents } from './warehouse-routing.module';
import { CommonModule } from '@angular/common';
import { NgxUploaderModule } from 'ngx-uploader';
import { NbDatepickerModule } from '@nebular/theme';
import { NbMomentDateModule } from '@nebular/moment';

@NgModule({
  imports: [
    ThemeModule,
    WarehouseRoutingModule,
    Ng2SmartTableModule,
    ToasterModule,
    CommonModule,
    NgxUploaderModule,
    NbDatepickerModule,
    NbMomentDateModule
   ],
  declarations: [
    ...routedComponents,
  ],
  providers: [
  ],
})
export class WarehouseModule { }
  