import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import {NoteDetail} from '../base/note-detail';
import { CONFIG } from '../../../app.config';
import { Http } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import {DetailService} from '../log/detail.service';
@Component({
    selector: 'ngx-modal',
    templateUrl: './detail.component.html',
    providers:  [ NoteDetail ]
  })
  export class DetailComponent {
    list:any =[];
    listDetail :   NoteDetail[];
    modalHeader: string;
    Id: string;
    Code: string;
    
    constructor(private activeDetail: NgbActiveModal,
      public _http: Http) { }
    private BaseUrl = CONFIG.baseURL + '/odata/Note';
    closeDetail() {
      this.activeDetail.close();
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
      })
           
    }
    setIdExcel(list: any){
      this.list = list;
      this.listDetail = list.Details;
      console.log(this.listDetail);
    }
  }
  