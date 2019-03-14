import {Component, OnInit} from '@angular/core';
import {Product} from '../base/product';
import {ProductService} from './product.service';

@Component({
  selector: 'create',
  templateUrl: './create.component.html',
  providers: [ProductService]
})

export class CreateComponent implements OnInit{
  list: Product[];

  constructor(private _productService: ProductService) {}

  ngOnInit(){
    this.getListProduct();
  }

  getListProduct(): void {
    this._productService.getList()
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
    this._productService.createProduct(event.newData).subscribe(
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
    this._productService.updateProduct(event.newData).subscribe(
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
    this._productService.deleteProduct(event.data.Id).subscribe(
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
        show: false
      },
      Code: {
        title: 'Code'
      },
      ProductName: {
        title: 'Sản phẩm'
      },
      Unit: {
        title: 'Đơn vị'
      },
      Price: {
        title: 'Đơn giá',
        valuePrepareFunction: (value) => { return value === 'Total'? value : Intl.NumberFormat().format(value)},
      },
      Note: {
        title: 'Ghi chú'
      }
    }
  };
}
