import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import {NoteDetail} from '../base/note-detail';
import { CONFIG } from '../../../app.config';
import { Http } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import {DetailService} from '../log/detail.service';
import { Warehouse } from '../base/warehouse';
import { ListService } from '../list/list.service';
import { Search } from '../base/search';

@Component({
    selector: 'ngx-modal',
    templateUrl: './printimport.component.html',
    providers:  [ NoteDetail,ListService ],
    styles: ['table tr#mainHeader { background-color: #404040 !important; color: white !important}']
  })
  export class PrintImportComponent {
    modalHeader: string;
    Id: string;
    Code: string;
    list:any =[];
    listDetail :NoteDetail[];
    warehouseList: Warehouse[];
    warehouseList1: Warehouse[];
    searchModel = new Search();

    constructor(private activeDetail: NgbActiveModal,
        public _http: Http,
        private _listService: ListService,) { }
      private BaseUrl = CONFIG.baseURL + '/odata/Note';
      closeDetail() {
        this.activeDetail.close();
      }
      PrintNote(printNote: string){
        let popupWinindow
        let innerContents = document.getElementById(printNote).innerHTML;
        popupWinindow = window.open('', '_blank', 'width=600,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
        popupWinindow.document.open();
        popupWinindow.document.write('<html><head></head><body onload="window.print()">' + innerContents + '</html>');
        popupWinindow.document.close();
      }
      setId(Id: string) {
        this.Id = Id;
        console.log(this.Id);
        this._http.get(this.BaseUrl + '('+ Id + ')?$expand=Details'  )
        .map(res => res.json()).subscribe((data: {Details:any}) => {
          console.log(data);
          this.list = data;

          this.listDetail = data.Details;
          console.log(this.listDetail);
          console.log("return list data " + this.list.Code)
        })
             
      }
      getWarehouse(Id: string): void {
        this.searchModel.Id = Id;
        this._listService.getListFilter(this.searchModel)
        .subscribe(
            value => {
            this.warehouseList = value.value[0];
            },
            error => {
            console.log(<any>error)
            }
        );
        console.log("warehouse "  + this.warehouseList);
      }    
      getWarehouse1(Id: string): void {
        this.searchModel.Id = Id;
        this._listService.getListFilter(this.searchModel)
        .subscribe(
            value => {
                console.log(value);
            this.warehouseList1 = value.value[0];
            },
            error => {
            console.log(<any>error)
            }
        );
        console.log("warehouse "  + this.warehouseList);
      }    
      
  }

