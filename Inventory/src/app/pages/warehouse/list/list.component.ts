import {Component, OnInit} from '@angular/core';
import {ListService} from '../list/list.service';
import {Warehouse} from '../base/warehouse';

@Component({
  selector: 'list',
  templateUrl: './list.component.html',
  providers:  [ ListService ]
})

export class ListComponent implements OnInit{
  list: Warehouse[];

  constructor(private _listService: ListService) {}

  ngOnInit(){
    this.getListWarehouse();
  }

  getListWarehouse(): void {
    this._listService.getList()
      .subscribe(
        value => {
          this.list = value.value;
        },
        error => {
          console.log(<any>error)
        }
      );
  }

  onCreateConfirm(event): void {
    delete event.newData.Id;
    this._listService.createWarehouse(event.newData).subscribe(
      data => {
          console.log("POST request is success!", data);
      },
      error => {
          console.log("ERROR", error);
      },
      () => {
        event.confirm.resolve(event.newData);
      }
    );
  }

  onEditConfirm(event): void {
    this._listService.updateWarehouse(event.newData).subscribe(
      data => {
          console.log("PUT request is success!", data);
         
      },
      error => {
          console.log("ERROR", error);
      },
      () => {
        event.confirm.resolve(event.newData);
      }
    );
  }

  onDeleteConfirm(event): void {
    this._listService.deleteWarehouse(event.data.Id).subscribe(
      data => {
          console.log("DELETE request is success!", data);
      },
      error => {
          console.log("ERROR", error);
      },
      () => {
        event.confirm.resolve(event.newData);
      }
    );
  }
  
  settings = {
    add: {
      addButtonContent: '<i class="nb-plus"></i>',
      createButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
      confirmCreate: true
    },
    edit: {
      editButtonContent: '<i class="nb-edit"></i>',
      saveButtonContent: '<i class="nb-checkmark"></i>',
      cancelButtonContent: '<i class="nb-close"></i>',
      confirmSave: true
    },
    delete: {
      deleteButtonContent: '<i class="nb-trash"></i>',
      confirmDelete: true,
    },
    columns: {
      Id: {
        title: 'Id',
        show: false,
      },
      Code:{
        title: 'Mã kho'
      },
      Name: {
        title: 'Tên kho'
      },
      Address: {
        title: 'Địa chỉ'
      },
      PhoneNumber: {
        title: 'Số ĐT'
      }
    }
  };
}
