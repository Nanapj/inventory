import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {ListComponent} from  './list/list.component';
import {ListDebtCollectComponent} from  './listdebtcollect/listdebtcollect.component';
import {CreateComponent} from  './create/create.component';
import { DebtNoteComponent } from './debtnote.component';
import {CreateCollectDebtNoteComponent} from  './createcollectdebtnote/createcollectdebtnote.component';
const routes: Routes = [
  {
    path: '',
    component: DebtNoteComponent,
    children: [
      {
        path: 'list',
        component: ListComponent,
      },
      {
        path: 'listdebtcollect',
        component: ListDebtCollectComponent,
      },
      {
        path: 'create',
        component: CreateComponent,
      },
      {
        path: 'createcollectdebtnote',
        component: CreateCollectDebtNoteComponent,
      },
    
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class DebtNoteRoutingModule { }
export const routedComponents = [
    DebtNoteComponent,
    ListComponent,
    ListDebtCollectComponent,
    CreateComponent,
    CreateCollectDebtNoteComponent
   

];