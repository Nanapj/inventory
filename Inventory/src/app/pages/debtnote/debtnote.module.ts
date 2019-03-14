import { NgModule } from '@angular/core';
import { ThemeModule } from '../../@theme/theme.module';
import { Ng2SmartTableModule, LocalDataSource } from 'ng2-smart-table';
import { DebtNoteRoutingModule, routedComponents } from './debtnote-routing.module';
import { ToasterModule } from 'angular2-toaster';
import { NbDatepickerModule } from '@nebular/theme';
import { NbMomentDateModule } from '@nebular/moment';

@NgModule({
  imports: [
    ThemeModule,
    DebtNoteRoutingModule,
    Ng2SmartTableModule,
    ToasterModule,
    NbDatepickerModule,
    NbMomentDateModule
   ],
  declarations: [
    ...routedComponents,
  ],
  providers: [
    
  ],
})
export class DebtNoteModule { }
  