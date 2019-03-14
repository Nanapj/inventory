import { Component, OnInit } from '@angular/core';
import { LogService } from '../log/log.service';
import { Log } from '../base/log';
import { DatePipe } from '@angular/common';
import { Warehouse } from '../base/warehouse';
import { Note } from '../base/note';
import { Search } from '../base/search';
import { ListService } from '../list/list.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DetailComponent } from './detail.component';
import { PrintExportComponent } from './print.component';
import { ToasterService, ToasterConfig, Toast, BodyOutputType } from 'angular2-toaster';
import { PrintImportComponent } from './printimport.component';
@Component({
  selector: 'log',
  templateUrl: './log.component.html',
  providers:  [ LogService, DatePipe, ListService, ToasterService ],
  styles: [".nb-theme-corporate select.form-control:not([size]):not([multiple]) { height: calc(2rem + 9px) !important }"]
})

export class LogComponent implements OnInit {
  list: Log[];
  listCheck : Log[];
  warehouseList: Warehouse[];
  model = new Note();
  searchModel = new Search();
  search = new Search();
  chuyen = "";
  xuat = "";
  nhap = "";  
  constructor (
    private _toasterService: ToasterService,
    private _logService: LogService, 
    private _datePipe: DatePipe,
    private _listService: ListService,
    private _DeteilService: NgbModal,
    private _PrintlService: NgbModal,
  ) {}

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
    start= "";
    end = "";
  ngOnInit(){
    this.getListLog();
    this.getListWarehouse();
  }
  pickerDate(event: any) {
    this.start = this._datePipe.transform(new Date(event.start), "yyyy-MM-dd");
    this.end = this._datePipe.transform(new Date(event.end), "yyyy-MM-dd");
    this.searchModel.from = this.start;
    this.searchModel.to = this.end;    
     console.log(this.start);
     console.log(this.end);
     this._logService.filterNote(this.searchModel)
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
    if (window.confirm('Bạn muốn xóa phiếu?')) {
      this._logService.deleteLog(event.data.Id).subscribe(
        data => {
            console.log("DELETE request is success!", data);
        },
        error => {
            console.log("ERROR", error);
        },
        () => {
          this.getListWarehouse();
        }
      );
      event.confirm.resolve();
    } else {
      event.confirm.reject();
    }
  }

  getListLog(): void {
    this._logService.getList()
      .subscribe(
        value => {
          this.list = value.value;
        },
        error => {
          console.log(<any>error)
        }
      );
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

  selectFromWarehouseHandler (event: any) {
    this.searchModel.SourceId = event.target.value;
    this._logService.filterNote(this.searchModel)
      .subscribe(
        value => {
          this.list = value.value;
        },
        error => {
          console.log(<any>error)
        }
      );
  }

  selectToWarehouseHandler (event: any) {
    this.searchModel.DestinationId = event.target.value;
    this._logService.filterNote(this.searchModel)
      .subscribe(
        value => {
          this.list = value.value;
          
        },
        error => {
          console.log(<any>error)
        }
      );
  }

  clickRow(event: any){
    console.log(event.data);
    this.searchModel.Id = event.data.Id;
    this.search.DestinationId = event.data.DestinationId;
    this.search.SourceId = event.data.SourceId;
    this.searchModel.Code = event.data.Code;
    if(event.data.SourceWarehouse == null && event.data.DestinationWarehouse != null )
    {
      this.xuat = "0";
      this.nhap = "1";
    }
    else if(event.data.SourceWarehouse != null && event.data.DestinationWarehouse == null)
    {
      this.xuat = "1";
      this.nhap = "0";
    }
    else if(event.data.SourceWarehouse != null && event.data.DestinationWarehouse != null)
    {
      this.xuat = "1";
      this.nhap = "1";
    }
    console.log("xuất = " + this.xuat);
    console.log("nhập = " + this.nhap);
  }
  

  detail() {
    const activeModal = this._DeteilService.open(DetailComponent, { size: 'lg', container: 'nb-layout' });
    activeModal.componentInstance.setId(this.searchModel.Id);
    activeModal.componentInstance.modalHeader = 'Chi tiết phiếu ' + this.searchModel.Code;
  }

  onConfirmListAction (event: any) {
    const activeModal = this._DeteilService.open(DetailComponent, { size: 'lg', container: 'nb-layout' });
    activeModal.componentInstance.setId(event.data.Id);
    activeModal.componentInstance.modalHeader = 'Chi tiết phiếu ' + event.data.Code;
    // const activeModal1 = this._PrintlService.open(PrintExportComponent, { size: 'lg', container: 'nb-layout' });
    // activeModal1.componentInstance.modalHeader = 'Chi tiết phiếu xuất' + this.searchModel.Code;
    // const activeModal2 = this._PrintlService.open(PrintImportComponent, { size: 'lg', container: 'nb-layout' });
    // activeModal2.componentInstance.modalHeader = 'Chi tiết phiếu nhập' + this.searchModel.Code;
  }

  // excelExport(){
  //   if(this.chuyen == "1")
  //   {
  //     this._logService.downloadDeliverNote(this.searchModel).subscribe(
  //       data => {
  //           console.log("POST request is success!", data);
  //           this.searchModel = new Search();
  //       },
  //       error => {
  //           console.log("ERROR", error);
  //       }
  //     );
  //   }
  //   else
  //   {
  //     this.showToast(this.type, this.title, "Chỉ chọn phiếu chuyển kho để xuất Excel");
  //   }
  // }
  
  printExportNote(){
    if(this.xuat == "1")
    {
      const activeModal = this._PrintlService.open(PrintExportComponent, { size: 'lg', container: 'nb-layout' });
      activeModal.componentInstance.setId(this.searchModel.Id);
      activeModal.componentInstance.getWarehouse(this.search.DestinationId);
      activeModal.componentInstance.getWarehouse1(this.search.SourceId);
      activeModal.componentInstance.modalHeader = 'Phiếu xuất' + this.searchModel.Code;
      
    }
    else
    {
      this.showToast(this.type, this.title, "Chỉ chọn in phiếu nhập");
    }
    
  }
  printImportNote(){
    if(this.nhap == "1")
    {
      const activeModal = this._PrintlService.open(PrintImportComponent, { size: 'lg', container: 'nb-layout' });
      activeModal.componentInstance.setId(this.searchModel.Id);
      activeModal.componentInstance.getWarehouse(this.search.DestinationId);
      activeModal.componentInstance.getWarehouse1(this.search.SourceId);
      activeModal.componentInstance.modalHeader = 'Phiếu nhập' + this.searchModel.Code;
    }
    else
    {
      this.showToast(this.type, this.title, "Chỉ chọn in phiếu xuất");
    }
    
  }
  // excelImport(){
  //   if(this.chuyen == "1")
  //   {
  //     this._logService.downloadReceiveNote(this.searchModel).subscribe(
  //       data => {
  //           console.log("POST request is success!", data);
  //           this.searchModel = new Search();
  //       },
  //       error => {
  //           console.log("ERROR", error);
  //       }
  //     );
  //   }
  //   else
  //   {
  //     this.showToast(this.type, this.title, "Chỉ chọn phiếu chuyển kho để xuất Excel");
  //   }
    
  // }

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
      CreatedDate: {
        title: 'Ngày tạo',
        valuePrepareFunction: (created) => {
          return this._datePipe.transform(new Date(created), 'dd/MM/yyyy');
        }
      },
      Code: {
        title: 'Mã phiếu'
      },
      SourceWarehouse: {
        title: 'Từ kho'
      },
      DestinationWarehouse: {
        title: 'Đến kho'
      },
      Total: {
        title: 'Tổng tiền',
        valuePrepareFunction: (value) => { return value === 'Total'? value : Intl.NumberFormat().format(value)}
      },
      Noted: {
        title: 'Ghi chú'
      },
      UserName: {
        title: 'Người tạo'
      }
    }
  };
}