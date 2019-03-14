import { NgModule } from '@angular/core';
import { ThemeModule } from '../../@theme/theme.module';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { ProductRoutingModule, routedComponents } from './product-routing.module';

@NgModule({
  imports: [
    ThemeModule,
    ProductRoutingModule,
    Ng2SmartTableModule,
  ],
  declarations: [
    ...routedComponents,
  ],
  providers: [
  ],
})
export class ProductModule { }