import {Component, OnInit} from '@angular/core';
import {LocalDataSource} from 'ng2-smart-table';
import {ToasterService, ToasterConfig, Toast, BodyOutputType} from 'angular2-toaster';
import {TransferService} from '../transfer/transfer.service';
import {Warehouse} from '../base/warehouse';
import {ListService} from '../list/list.service';
import {Product} from '../../product/base/product';
import {Note} from '../../warehouse/base/note';
import {NoteDetail} from '../../warehouse/base/note-detail';
import {ProductService} from '../../product/create/product.service';
import { Search } from '../../warehouse/base/search';
import 'style-loader!angular2-toaster/toaster.css';
import {DashboardService} from '../../dashboard/dashboard.service';

@Component({
  selector: 'transfer',
  templateUrl: './transfer.component.html',
  providers: [ListService, ProductService, TransferService, ToasterService, DashboardService]
})

export class TransferComponent implements OnInit {
  warehouseList: Warehouse[];
  productList: Product[];
  model = new Note();
  detail = new NoteDetail();
  render = new LocalDataSource(this.model.Details);
  searchModel = new Search();
  listSpTonKho: Product[];
  typeSelect: -1;
  warehouseSourceSelect: 0;
  warehouseDistSelect: 0;
  productSelect: 0;

  constructor(
    private _toasterService: ToasterService,
    private _importService: TransferService,
    private _listService: ListService,
    private _productService: ProductService,
    private _dashboardService: DashboardService,) {}

  ngOnInit(){
    this.getListWarehouse();
    this.getListProduct();
    this.typeSelect= -1;
    this.warehouseSourceSelect = 0;
    this.warehouseDistSelect= 0;
    this.productSelect= 0;
    this.model.ProductType = -1; // Khởi tạo hình thức
  }

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
  TongTien = 0;

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
    else if(this.model.SourceId == null || this.model.SourceId == '0' ){
      this.showToast(this.type, this.title,this.contentKho );
    }
    else if(this.model.DestinationId == null || this.model.DestinationId == '0' ){
      this.showToast(this.type, this.title,this.contentKho );
    }
    else if(this.detail.Quantity == null || this.detail.Quantity == 0)
    {
      this.showToast(this.type, this.title, this.contentSl);
    }
    else if(this.detail.Price == null || this.detail.Price == '0')
    {
      this.showToast(this.type, this.title, this.contentSt);
    }else{
      var flag = 0;
      this.listSpTonKho.forEach((o : any) => {
        if((this.detail.ProductName == o.ProductName) && (this.detail.Quantity > o.Quantity)) {
          console.log("Sản phẩm " + o.ProductName + " tồn kho chỉ còn " + o.Quantity);
          this.showToast(this.type, this.title, "Sản phẩm " + o.ProductName + " tồn kho chỉ còn " + o.Quantity + " sản phẩm");
          flag = 1;
        }
      });
  
      if (!this.listSpTonKho.some((item) => item.ProductName == this.detail.ProductName)) {
        flag = 1;
        this.showToast(this.type, this.title, "Sản phẩm này không còn trong kho!");
      }
  
      if(flag == 1) {
        return;
      }
  
     
      this.model.Details.push(this.detail);
      this.detail = new NoteDetail(); // Refresh bo nho bien detail
      this.render = new LocalDataSource(this.model.Details); // Binding data moi
      console.log(this.model.Details);
      // this.warehouseSourceSelect = 0;
      // this.warehouseDistSelect= 0;
      this.productSelect= 0;
      this.resetTong();
    }
    
  }

  addNote(): void {
    if(this.model.DestinationId == null || this.model.DestinationId == '0' ){
      this.showToast(this.type, this.title,this.contentKho );
    }
    else if(this.model.SourceId == null || this.model.SourceId == '0' ){
      this.showToast(this.type, this.title,this.contentKho );
    }
    else if(this.model.Details == null)
    {
      this.showToast(this.type, this.title, "Chọn sản phẩm");
    }
    if(this.model.ProductType == -1){
        this.showToast(this.type, this.title, "Yêu cầu chọn hình thức!");
        return;
    }
    if(this.model.ProductType == 0){
      if (window.confirm('Bạn có muốn tạo phiếu công nợ không?')) {
        this._importService.createNote(this.model).subscribe(
          data => {
              console.log("POST request is success!", data);
              this.showToast('success', this.title, "Tạo phiếu thành công");
                this.model = new Note();
                this.detail = new NoteDetail();
              },
          error => {
              console.log("ERROR", error);
          }
        );
        
      } 
    }
    else
    {
      this._importService.createNote(this.model).subscribe(
        data => {
            console.log("POST request is success!", data);
            this.showToast('success', this.title, "Tạo phiếu thành công");
              this.model = new Note();
              this.detail = new NoteDetail();
            },
        error => {
            console.log("ERROR", error);
        }
      );
      this.getListWarehouse();
      this.getListProduct();
      this.typeSelect= -1;
      this.warehouseSourceSelect = 0;
      this.warehouseDistSelect= 0;
      this.productSelect= 0;
      this.model.ProductType = -1;
      this.render = null;  
    }
    
    
  }
  
  selectProductHandler (event: any) {
    this.detail.ProductId = event.target.value;
    this.detail.ProductName = event.target['options'][event.target['options'].selectedIndex].text
  }

  selectWarehouseDistHandler (event: any) {
    if(this.model.SourceId == event.target.value){
        this.showToast(this.type, this.title, this.content);
    }else{
        this.model.DestinationId = event.target.value;
    }
  }

  selectWarehouseSourceHandler (event: any) {
    this.model.SourceId = event.target.value;
    this.searchModel.SourceId = event.target.value;
    this._dashboardService.getFilterList1(this.searchModel).subscribe(
      value => {
        if (value.value[0] == null)
        {
          this.listSpTonKho= [];

        }else
        {
          this.listSpTonKho = value.value[0].StockList;
        }
      },
      error => {
        console.log(<any>error)
      }
    );
  }

  selectTypeHandler (event: any) {
      this.model.ProductType = event.target.value;
      console.log( this.model.ProductType);
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
