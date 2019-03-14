import {Component, OnInit} from '@angular/core';
import {ListService} from '../list/list.service';
import {DebtNote} from '../base/debtnote';
import { DatePipe } from '@angular/common';
import { Search } from '../../warehouse/base/search';

@Component({
  selector: 'listdebtcollect',
  templateUrl: './listdebtcollect.component.html',
  providers:  [ ListService, DatePipe ]
})

export class ListDebtCollectComponent implements OnInit{
  list: DebtNote[];
  searchModel = new Search();

  constructor(private _listService: ListService,
    private _datePipe: DatePipe,) {}
    start= "";
    end = "";

  ngOnInit(){
    this.getListDebtNote();
  }

  getListDebtNote(): void {
    this._listService.getListCollect()
      .subscribe(
        value => {
          this.list = value.value;
        },
        error => {
          console.log(<any>error)
        }
      );
  }
  pickerDate(event: any) {
    this.start = this._datePipe.transform(new Date(event.start), "yyyy-MM-dd");
    this.end = this._datePipe.transform(new Date(event.end), "yyyy-MM-dd");
    this.searchModel.from = this.start;
    this.searchModel.to = this.end;    
    console.log(this.start);
    console.log(this.end);
    this._listService.getListCollectFilter(this.searchModel)
    .subscribe(
      value => {
        this.list = value.value;
      },
      error => {
        console.log(<any>error)
      }
    );
  }
onDeleteConfirm(event): void {
    this._listService.deleteDebtNote(event.data.Id).subscribe(
      data => {
          console.log("DELETE request is success!", data);
      },
      error => {
          console.log("ERROR", error);
      },
      () => {
        this.getListDebtNote();
      }
    );
  }
  
  settings = {
    actions: {
      add: false,
      edit: false,
    },
    delete: {
      deleteButtonContent: '<i class="nb-trash"></i>',
      confirmDelete: true,
    },
    columns: {
      CreatedDate: {
        title: 'Ngày tạo',
        valuePrepareFunction: (created) => {
          return this._datePipe.transform(new Date(created), 'dd/MM/yyyy');
        }
      },
      Code: {
        title: 'Số phiếu',
       
      },
      PayWarehouse: {
        title: 'Tên kho'
      },
      Total: {
        title: 'Số tiền',
        valuePrepareFunction: (value) => { return value === 'Total'? value : Intl.NumberFormat().format(value)}
      },
      PayMethod: {
        title: 'Hình thức thanh toán'
      },
      Noted: {
        title: 'Ghi chú'
      }
    }
  };
}
