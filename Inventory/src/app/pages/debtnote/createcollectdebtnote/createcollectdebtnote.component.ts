import {Component, OnInit} from '@angular/core';
import {CreateService} from '../../debtnote/create/create.service';
import {Warehouse} from '../../warehouse/base/warehouse';
import {ListService} from '../../warehouse/list/list.service';
import {Product} from '../../product/base/product';
import {DebtNote} from '../base/debtnote';
import {PaymentType} from '../base/paymenttype';
import {ToasterService, ToasterConfig, Toast, BodyOutputType} from 'angular2-toaster';

@Component({
  selector: 'createcollectdebtnote',
  templateUrl: './createcollectdebtnote.component.html',
  providers: [ListService, CreateService, ToasterService]
})

export class CreateCollectDebtNoteComponent implements OnInit {
  warehouseList: Warehouse[];
  productList: Product[];
  model = new DebtNote();
  paymenttypeList: PaymentType[];
  
  
  constructor(
    private _toasterService: ToasterService,
    private _createService: CreateService,
    private _listService: ListService,
    ) {}

  ngOnInit(){
    this.getListWarehouse();
    this.getListPaymentType();

  }

  config: ToasterConfig;
  position = 'toast-top-right';
  animationType = 'fade';
  title = 'Thông báo';
  timeout = 5000;
  toastsLimit = 5;
  type = 'success';
  isNewestOnTop = true;
  isHideOnClick = true;
  isDuplicatesPrevented = false;
  isCloseButton = true;

  addDebtNote(): void {
    if(this.model.WarehouseId == null)
    {
      this.showToast("warning", "Chú ý", "Không để trống kho");
    }
    else if(this.model.Total == null ||this.model.Total <= "0" )
    {
      this.showToast("warning", "Chú ý", "Không để tổng tiền");
    }
    else if(this.model.Total <= "0" )
    {
      this.showToast("warning", "Chú ý", "Không nhập số tiền âm ");
    }
    else if(this.model.PaymentTypeId == null )
    {
      this.showToast("warning", "Chú ý", "Không để trống hình thức thanh toán");
    }
    else
    {
      this.model.isOwed = "false"; 
      this._createService.createDebtNote(this.model).subscribe(
        data => {
            console.log("POST request is success!", data);
            this.model = new DebtNote();
            this.showToast(this.type, this.title, "Tạo phiếu thành công");
        },
        error => {
            console.log("ERROR", error);
        }
      );
      this.getListWarehouse();
      this.getListPaymentType();
    }
  }
  
   selectWarehouseHandler (event: any) {
    this.model.WarehouseId = event.target.value;
    
  }
  selectPaymentTypeHandler (event: any) {
    this.model.PaymentTypeId = event.target.value;
    
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
  getListPaymentType(): void {
    this._createService.getListPayment()
      .subscribe(
        value => {
          this.paymenttypeList = value.value;
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
}
