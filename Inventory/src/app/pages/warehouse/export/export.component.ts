import {Component, EventEmitter } from '@angular/core';
import {LocalDataSource} from 'ng2-smart-table';
import {ExportService} from '../export/export.service';
import {Warehouse} from '../base/warehouse';
import {ListService} from '../list/list.service';
import {Product} from '../../product/base/product';
import {Note} from '../../warehouse/base/note';
import {NoteDetail} from '../../warehouse/base/note-detail';
import {ProductService} from '../../product/create/product.service';
import {ToasterService, ToasterConfig, Toast, BodyOutputType} from 'angular2-toaster';
import {DashboardService} from '../../dashboard/dashboard.service';
import { Search } from '../../warehouse/base/search';
import { UploadOutput, UploadInput, UploadFile, humanizeBytes, UploaderOptions } from 'ngx-uploader';
import { Http, Response, RequestOptions } from '@angular/http';
import { CONFIG } from '../../../app.config';
import { DatePipe } from '@angular/common';
import { DetailComponent } from '../../warehouse/log/detail.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'export',
  templateUrl: './export.component.html',
  providers: [ListService, ProductService, ExportService, ToasterService, DashboardService, DatePipe]
})
export class ExportComponent {
  options: UploaderOptions;
  formData = new FormData();
  files: UploadFile[];
  uploadInput: EventEmitter<UploadInput>;
  humanizeBytes: Function;
  dragOver: boolean;
  warehouseList: Warehouse[];
  productList: Product[];
  model = new Note();
  detail = new NoteDetail();
  searchModel = new Search();
  listSpTonKho: Product[];
  render = new LocalDataSource(this.model.Details);
  listExcel: Note[];
  productSelect: 0;
  warehouseSelect: 0;

  constructor(
    private _toasterService: ToasterService,
    private _exportService: ExportService,
    private _listService: ListService,
    private _productService: ProductService,
    private _dashboardService: DashboardService,
    private _http: Http,
    private _datePipe: DatePipe,
    private _DeteilService: NgbModal,
  ) {
    this.options = { concurrency: 1, maxUploads: 3 };
    this.files = []; // local uploading files array
    this.uploadInput = new EventEmitter<UploadInput>(); // input events, we use this to emit data to ngx-uploader
    this.humanizeBytes = humanizeBytes;
  }
   config: ToasterConfig;
  position = 'toast-top-right';
  animationType = 'fade';
  title = 'Chú ý!';
  content = `Bạn vừa chọn kho nguồn và kho đích trùng nhau!`;
  timeout = 5000;
  toastsLimit = 5;
  type = 'warning';
  isNewestOnTop = true;
  isHideOnClick = true;
  isDuplicatesPrevented = false;
  isCloseButton = true;
  contentKho = `Không để trống kho`;
  contentSp = `Chọn sản phẩm`;
  contentSl = `Nhập số lượng sản phẩm`;
  contentSt = `Nhập giá tiền`;
  TongTien = 0;

  ngOnInit(){
    this.getListWarehouse();
    this.getListProduct();
    this.productSelect = 0;
    this.warehouseSelect = 0;
  }  
  clickRow(event: any){
    console.log(event.data.Details);
    const activeModal = this._DeteilService.open(DetailComponent, { size: 'lg', container: 'nb-layout' });
    activeModal.componentInstance.setIdExcel(event.data);
    activeModal.componentInstance.modalHeader = 'Chi tiết phiếu ';
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
    else if(this.detail.Quantity == null || this.detail.Quantity == 0)
    {
      this.showToast(this.type, this.title, this.contentSl);
    }
    else if(this.detail.Price == null )
    {
      this.showToast(this.type, this.title, this.contentSt);
    }
    else if(this.detail.Price < "0")
    {
      this.showToast(this.type, this.title, "Không nhập số tiền âm");
    }
    else if(this.detail.Quantity < 0){
      this.showToast(this.type, this.title, "Không nhập số lượng âm");
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
      this.productSelect = 0; 
      this.warehouseSelect = 0;
      console.log(this.model.Details);
      this. resetTong();
    }
  }
  
  resetTong(){
    this.TongTien = 0;
    this.model.Details.forEach((o: any) => {
      
      this.TongTien += o.Price * o.Quantity;
    });
    console.log(this.TongTien)
  }
  addNote(): void {
    if(this.model.SourceId == null || this.model.SourceId == '0' ){
      this.showToast(this.type, this.title,this.contentKho );
    }
    else if(this.model.Details == null)
    {
      this.showToast(this.type, this.title, "Chọn sản phẩm");
    }
    else{
      this._exportService.createNote(this.model).subscribe(
        data => {
          console.log(this.model);
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
  addNoteExcel(): void{
    this.listExcel.forEach((o : any) => {
      delete o.Id;
      delete o.LastModifiedDate;
      delete o.DestinationId;
      delete o.Source;
      delete o.Destination;
      delete o.CreatedDate;
      delete o.ProductType;
      delete o.Details.Id;
      delete o.Details.NoteId;
      delete o.Details.LastModifiedDate;
      delete o.Details.CreatedDate;
      
      o.Details.forEach((j : any) => {
        delete j.Id;
        delete j.LastModifiedDate;
        delete j.CreatedDate;
        delete j.NoteId;

       
      })
      this._exportService.createNote(o).subscribe(
        data => {
            console.log("POST request is success!", data);
            this.listExcel = [];
            this.showToast("success", "Thông báo", "Tạo phiếu thành công");
           
        },
        error => {
            console.log("ERROR", error);
            this.listExcel = [];
            this.showToast("error", "Thông báo", "Tạo phiếu thành công nhưng bị lỗi");
        }
      );
    })
    
  }
  selectProductHandler (event: any) {
    this.detail.ProductId = event.target.value;
    this.detail.ProductName = event.target['options'][event.target['options'].selectedIndex].text
  }

  selectWarehouseHandler (event: any) {
    this.model.SourceId = event.target.value;
    this.searchModel.Id = event.target.value;
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
    );;

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

  onUploadOutput(output: UploadOutput): void {
    switch (output.type) {
      case 'allAddedToQueue':
          // uncomment this if you want to auto upload files when added
          // const event: UploadInput = {
          //   type: 'uploadAll',
          //   url: '/upload',
          //   method: 'POST',
          //   data: { foo: 'bar' }
          // };
          // this.uploadInput.emit(event);
        break;
      case 'addedToQueue':
        if (typeof output.file !== 'undefined') {
          this.files.push(output.file);
        }
        break;
      case 'uploading':
        if (typeof output.file !== 'undefined') {
          // update current data in files array for uploading file
          const index = this.files.findIndex((file) => typeof output.file !== 'undefined' && file.id === output.file.id);
          this.files[index] = output.file;
        }
        break;
      case 'removed':
        // remove file from array when removed
        this.files = this.files.filter((file: UploadFile) => file !== output.file);
        break;
      case 'dragOver':
        this.dragOver = true;
        break;
      case 'dragOut':
      case 'drop':
        this.dragOver = false;
        break;
      case 'done':
        // The file is downloaded
        break;
    }
  }

  startUpload(): void {
    for (var i = 0; i < this.files.length; i++) {
      this.formData.append(this.files[i].name, this.files[i].nativeFile);
    }
    this._http.post(CONFIG.baseURL + '/Excel/UploadNote', this.formData).subscribe(x => 
      {
        console.log(x);
       this.listExcel = x.json();
      var count = 0;
          this.listExcel.forEach((o: any) => {
            o.Id = count;
            o.Details.forEach((j : any) => {
             j.NoteId = count;
            });
            count++;
          });
          console.log(this.listExcel);
      });
      
  }
  
  cancelUpload(id: string): void {
    this.uploadInput.emit({ type: 'cancel', id: id });
  }

  removeFile(id: string): void {
    this.uploadInput.emit({ type: 'remove', id: id });
  }

  removeAllFiles(): void {
    this.uploadInput.emit({ type: 'removeAll' });
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
  settings1 = {
    mode: 'external',
    actions: false,
    hideSubHeader: true,
    columns: {
      
      Code: {
        title: 'Mã phiếu',
        filter: false
      },
      SourceWarehouse : {
        title: 'Từ kho',
        filter: false
      },
      Total: {
        title: 'Tổng tiền',
        valuePrepareFunction: (value) => { return value === 'Total'? value : Intl.NumberFormat().format(value)},
        filter: false
      },
      Noted: {
        title: 'Ghi chú',
        filter: false
      }
    }
  };
}
