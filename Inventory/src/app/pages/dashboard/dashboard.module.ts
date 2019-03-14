import { NgModule } from '@angular/core';

import { ThemeModule } from '../../@theme/theme.module';
import { DashboardComponent } from './dashboard.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NbDatepickerModule } from '@nebular/theme';
import { NbMomentDateModule } from '@nebular/moment';
import { Ng2SmartTableModule, LocalDataSource } from 'ng2-smart-table';

@NgModule({
  imports: [
    ThemeModule,
    NgbModule.forRoot(),
    Ng2SmartTableModule,
    NbDatepickerModule,
    NbMomentDateModule
  ],
  declarations: [
    DashboardComponent,
  ]
})
export class DashboardModule { }
