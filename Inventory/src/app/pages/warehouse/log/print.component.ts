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
    templateUrl: './print.component.html',
    providers:  [ NoteDetail,ListService ],
    styles: ['table tr#mainHeader { background-color: #404040 !important; color: white !important}']
  })
export class PrintExportComponent {
  modalHeader: string;
  Id: string;
  Code: string;
  list:any =[];
  listDetail :NoteDetail[];
  TienChu: string;
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
  DocSo3ChuSo(baso: any)
  {var ChuSo=new Array(" không "," một "," hai "," ba "," bốn "," năm "," sáu "," bảy "," tám "," chín ");
      var tram;
      var chuc;
      var donvi;
      var KetQua="";
      tram=parseInt((baso/100).toString());
      chuc=parseInt(((baso%100)/10).toString());
      donvi=baso%10;
      if(tram==0 && chuc==0 && donvi==0) return "";
      if(tram!=0)
      {
          KetQua += ChuSo[tram] + " trăm ";
          if ((chuc == 0) && (donvi != 0)) KetQua += " linh ";
      }
      if ((chuc != 0) && (chuc != 1))
      {
              KetQua += ChuSo[chuc] + " mươi";
              if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh ";
      }
      if (chuc == 1) KetQua += " mười ";
      switch (donvi)
      {
          case 1:
              if ((chuc != 0) && (chuc != 1))
              {
                  KetQua += " mốt ";
              }
              else
              {
                  KetQua += ChuSo[donvi];
              }
              break;
          case 5:
              if (chuc == 0)
              {
                  KetQua += ChuSo[donvi];
              }
              else
              {
                  KetQua += " lăm ";
              }
              break;
          default:
              if (donvi != 0)
              {
                  KetQua += ChuSo[donvi];
              }
              break;
          }
      return KetQua;
  }
  DocTienBangChu(SoTien)
  {
    var Tien=new Array( "", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ");
      var lan=0;
      var i=0;
      var so=0;
      var KetQua="";
      var tmp="";
      var ViTri = new Array();
      if(SoTien<0) return "Số tiền âm !";
      if(SoTien==0) return "Không đồng !";
      if(SoTien>0)
      {
          so=SoTien;
      }
      else
      {
          so = -SoTien;
      }
      if (SoTien > 8999999999999999)
      {
          //SoTien = 0;
          return "Số quá lớn!";
      }
      ViTri[5] = Math.floor(so / 1000000000000000);
      if(isNaN(ViTri[5]))
          ViTri[5] = "0";
      so = so - parseFloat(ViTri[5].toString()) * 1000000000000000;
      ViTri[4] = Math.floor(so / 1000000000000);
       if(isNaN(ViTri[4]))
          ViTri[4] = "0";
      so = so - parseFloat(ViTri[4].toString()) * 1000000000000;
      ViTri[3] = Math.floor(so / 1000000000);
       if(isNaN(ViTri[3]))
          ViTri[3] = "0";
      so = so - parseFloat(ViTri[3].toString()) * 1000000000;
      ViTri[2] = parseInt((so / 1000000).toString());
       if(isNaN(ViTri[2]))
          ViTri[2] = "0";
      ViTri[1] = parseInt(((so % 1000000) / 1000).toString());
       if(isNaN(ViTri[1]))
          ViTri[1] = "0";
      ViTri[0] = parseInt((so % 1000).toString());
    if(isNaN(ViTri[0]))
          ViTri[0] = "0";
      if (ViTri[5] > 0)
      {
          lan = 5;
      }
      else if (ViTri[4] > 0)
      {
          lan = 4;
      }
      else if (ViTri[3] > 0)
      {
          lan = 3;
      }
      else if (ViTri[2] > 0)
      {
          lan = 2;
      }
      else if (ViTri[1] > 0)
      {
          lan = 1;
      }
      else
      {
          lan = 0;
      }
      for (i = lan; i >= 0; i--)
      {
         tmp = this.DocSo3ChuSo(ViTri[i]);
         KetQua += tmp;
         if (ViTri[i] > 0) KetQua += Tien[i];
         if ((i > 0) && (tmp.length > 0)) KetQua += ',';//&& (!string.IsNullOrEmpty(tmp))
      }
     if (KetQua.substring(KetQua.length - 1) == ',')
     {
          KetQua = KetQua.substring(0, KetQua.length - 1);
     }
     KetQua = KetQua.substring(1,2).toUpperCase()+ KetQua.substring(2);
     return KetQua;
  }  
  setId(Id: string) {
    this.Id = Id;
    console.log(this.Id);
    this._http.get(this.BaseUrl + '('+ Id + ')?$expand=Details'  )
    .map(res => res.json()).subscribe((data: {Details:any}) => {
        console.log(data);
        this.list = data;
        this.listDetail = data.Details;
        this.TienChu = this.DocTienBangChu(this.list.Total);
        this.searchModel.Id =  this.list.DestinationId;
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

