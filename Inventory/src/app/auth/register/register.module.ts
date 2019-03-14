import { NgModule } from '@angular/core';

import { ThemeModule } from '../../@theme/theme.module';
import { RegisterComponent } from './register.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  imports: [
    ThemeModule,
    NgbModule.forRoot(),
  ],
  declarations: [
    RegisterComponent,
  ]
})
export class RegisterModule { }
