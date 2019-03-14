import {Component, OnInit} from '@angular/core';
import {LocalDataSource} from 'ng2-smart-table';
import {ImportService} from '../import/import.service';
import {Warehouse} from '../base/warehouse';
import {ListService} from '../list/list.service';
import {Product} from '../../product/base/product';
import {Note} from '../../warehouse/base/note';
import {NoteDetail} from '../../warehouse/base/note-detail';
import {ProductService} from '../../product/create/product.service';
import {ToasterService, ToasterConfig, Toast, BodyOutputType} from 'angular2-toaster';
import 'style-loader!angular2-toaster/toaster.css';
@Component({
  selector: 'import',
  templateUrl: './import.component.html',
  providers: [ListService, ProductService, ImportService, ToasterService]
})

export class ImportComponent implements OnInit {
  warehouseList: Warehouse[];
  productList: Product[];
  model = new Note();
  detail = new NoteDetail();
  render = new LocalDataSource(this.model.Details);
  TongTien = 0;
  productSelect: 0;
  warehouseSelect: 0;
  constructor(
    private _toasterService: ToasterService,
    private _importService: ImportService,
    private _listService: ListService,
    private _productService: ProductService) {}

    config: ToasterConfig;
    position = 'toast-top-right';
    animationType = 'fade';
    title = 'Chú ý!';
    content = `Bạn vừa chọn kho nguồn và kho đích trùng nhau!`;
    timeout = 5000;
    toastsLimit = 5;
    type = 'warning';
    contentKho = `Không để trống kho`;
    contentSp = `Chọn sản phẩm`;
    contentSl = `Nhập số lượng sản phẩm`;
    contentSt = `Nhập giá tiền`;
    isNewestOnTop = true;
    isHideOnClick = true;
    isDuplicatesPrevented = false;
    isCloseButton = true;

  ngOnInit(){
    this.getListWarehouse();
    this.getListProduct();
    this.productSelect = 0;
    this.warehouseSelect = 0;
  }
  resetTong(){
    this.TongTien = 0;
    this.model.Details.forEach((o: any) => {
      
      this.TongTien += o.Price * o.Quantity;
    });
    console.log(this.TongTien)
  }

  addDetail(): void {
    if(this.model.Details == null)
    {
      this.model.Details = new Array<NoteDetail>();
    }
    if (this.detail.ProductId == null || this.detail.ProductId == '0')
    {
      this.showToast(this.type, this.title, this.contentSp);
    }
    else if(this.detail.Quantity == null || this.detail.Quantity == 0)
    {
      this.showToast(this.type, this.title, this.contentSl);
    }
    else if(this.detail.Price == null)
    {
      this.showToast(this.type, this.title, this.contentSt);
    }
    else if(this.detail.Price < "0")
    {
      this.showToast(this.type, this.title, "Không nhập số tiền âm");
    }
    else if(this.detail.Quantity < 0){
      this.showToast(this.type, this.title, "Không nhập số lượng âm");
    }
    else{
      this.model.Details.push(this.detail);

      this.detail = new NoteDetail(); // Refresh bo nho bien detail
      this.render = new LocalDataSource(this.model.Details); // Binding data moi
      this.productSelect = 0; 
      this.warehouseSelect = 0;
      console.log(this.model.Details);
      this. resetTong();
    }
    
  }
  addNote(): void {
    if(this.model.DestinationId == null || this.model.DestinationId == '0' ){
      this.showToast(this.type, this.title,this.contentKho );
    }
    else if(this.model.Details == null)
    {
      this.showToast(this.type, this.title, "Chọn sản phẩm");
    }
    else{
      this._importService.createNote(this.model).subscribe(
        data => {
            console.log("POST request is success!", data);
            this.model = new Note();
            this.detail = new NoteDetail();
            this.showToast("success", "Thông báo", "Tạo phiếu thành công");
            
        },
        error => {
            console.log("ERROR", error);
        }
      );
      this.TongTien = 0;
      this.getListWarehouse();
      this.getListProduct();
      this.productSelect = 0;
      this.warehouseSelect = 0;
      this.render = null;
    }
    
  }
  
  selectProductHandler (event: any) {
    this.detail.ProductId = event.target.value;
    this.detail.ProductName = event.target['options'][event.target['options'].selectedIndex].text
  }

  selectWarehouseHandler (event: any) {
    this.model.DestinationId = event.target.value;
    
  }

  getListWarehouse(): void {
    this._listService.getList()
      .subscribe(
        value => {
          this.warehouseList = value.value;
        },
        error => {
          console.log(<any>error)
        }
      );
  }
  onDeleteConfirm(event): void {
    console.log(event);
    event.confirm.resolve();
    this.model.Details.forEach((o : any, index) => {
      if (o.ProductId == event.data.ProductId){
        this.model.Details.splice(index,1)
      }
    });
    this. resetTong();
    console.log(this.model.Details);
  }
  

  getListProduct(): void {
    this._productService.getList()
      .subscribe(
        value => {
          this.productList = value.value;
        },
        error => {
          console.log(<any>error)
        }
      );
  }
  private showToast(type: string, title: string, body: string) {
    this.config = new ToasterConfig({
      positionClass: this.position,
      timeout: this.timeout,
      newestOnTop: this.isNewestOnTop,
      tapToDismiss: this.isHideOnClick,
      preventDuplicates: this.isDuplicatesPrevented,
      animation: this.animationType,
      limit: this.toastsLimit,
    });

    const toast: Toast = {
      type: type,
      title: title,
      body: body,
      timeout: this.timeout,
      showCloseButton: this.isCloseButton,
      bodyOutputType: BodyOutputType.TrustedHtml,
    };

    this._toasterService.pop(toast);
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
    hideSubHeader: true,
    columns: {
      ProductName: {
        title: 'Sản phẩm',
        filter: false
      },
      Quantity: {
        title: 'Số lượng',
        filter: false
      },
      Price: {
        title: 'Số tiền',
        filter: false,
        valuePrepareFunction: (value) => { return value === 'Total'? value : Intl.NumberFormat().format(value)},
      }
    }
  };
}
