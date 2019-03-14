import { NgModule } from '@angular/core';

import { AuthComponent } from './auth.component';
import { LoginModule } from './login/login.module';
import { RegisterModule } from './register/register.module';
import { AuthRoutingModule } from './auth-routing.module';
import { ThemeModule } from '../@theme/theme.module';
import { FormsModule }   from '@angular/forms';
import { NbAuthBlockComponent } from './auth-block/auth-block.component';

const Auth_COMPONENTS = [
    AuthComponent,
    NbAuthBlockComponent
];

@NgModule({
  imports: [
    AuthRoutingModule,
    ThemeModule,
    LoginModule,
    RegisterModule,
    FormsModule
  ],
  declarations: [
    ...Auth_COMPONENTS,
  ]
})
export class AuthModule {
}
