/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { APP_BASE_HREF } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { CoreModule } from './@core/core.module';
import { FormsModule }   from '@angular/forms';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { ThemeModule } from './@theme/theme.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DetailComponent } from '../app/pages/warehouse/log/detail.component';
import { PrintExportComponent } from '../app/pages/warehouse/log/print.component';
import { PrintImportComponent } from '../app/pages/warehouse/log/printimport.component';
import { AuthGuard } from './auth/auth-guard.service';
import { NgxUploaderModule } from 'ngx-uploader';
import { NbDatepickerModule } from '@nebular/theme';

@NgModule({
  declarations: [AppComponent, DetailComponent,PrintExportComponent, PrintImportComponent],
  imports: [
    BrowserModule,
    NgxUploaderModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    HttpModule,
    FormsModule,

    NgbModule.forRoot(),
    ThemeModule.forRoot(),
    CoreModule.forRoot(),
    NbDatepickerModule.forRoot(),
  ],
  bootstrap: [AppComponent],
  providers: [
    { provide: APP_BASE_HREF, useValue: '/' },
    AuthGuard
  ],
   entryComponents: [
    DetailComponent,
    PrintExportComponent,
    PrintImportComponent
  ]
})
export class AppModule {
}
