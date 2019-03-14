import { NgModule } from '@angular/core';

import { ThemeModule } from '../../@theme/theme.module';
import { LoginComponent } from './login.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  imports: [
    ThemeModule,
    NgbModule.forRoot(),
  ],
  declarations: [
    LoginComponent,
  ]
})
export class LoginModule { }
