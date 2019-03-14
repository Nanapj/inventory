import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WarehouseComponent } from './warehouse.component';
import { ListComponent } from './list/list.component';
import { LogComponent } from './log/log.component';
import { ImportComponent } from './import/import.component';
import { ExportComponent } from './export/export.component';
import { TransferComponent } from './transfer/transfer.component';

const routes: Routes = [
  {
    path: '',
    component: WarehouseComponent,
    children: [
      {
        path: 'log',
        component: LogComponent,
      },
      {
        path: 'list',
        component: ListComponent,
      },
      {
        path: 'import',
        component: ImportComponent,
      },
      {
        path: 'export',
        component: ExportComponent,
      },
      {
        path: 'transfer',
        component: TransferComponent,
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class WarehouseRoutingModule { }
export const routedComponents = [
  WarehouseComponent,
  ListComponent,
  LogComponent,
  ExportComponent,
  ImportComponent,
  TransferComponent
];