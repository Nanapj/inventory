import {Component, OnInit} from '@angular/core';
import { NbThemeService } from '@nebular/theme';
import { takeWhile } from 'rxjs/operators/takeWhile' ;
import {DashboardService} from '../dashboard/dashboard.service';
import { Product } from './base/product';
import {Warehouse} from '../warehouse/base/warehouse';
import {ListService} from '../warehouse/list/list.service';
import { Search } from '../warehouse/base/search';
import { DebtNote } from '../DebtNote/base/debtnote';
import { DatePipe } from '@angular/common';
import { throwIfAlreadyLoaded } from '../../@core/module-import-guard';

interface CardSettings {
  title: string;
  iconClass: string;
  type: string;
}

@Component({
  selector: 'ngx-dashboard',
  
  templateUrl: './dashboard.component.html',
  providers: [DashboardService, ListService, DatePipe]
})
export class DashboardComponent implements OnInit {
  list: Product[];
  debt: DebtNote[];
  start= "";
  end = "";
  startDebt= "";
  endDebt = "";
  TongNo = 0;
  
  listWarehouse: Warehouse[];
  searchModel = new Search();
  private alive = true;

  lightCard: CardSettings = {
    title: 'Light',
    iconClass: 'nb-lightbulb',
    type: 'primary',
  };
  rollerShadesCard: CardSettings = {
    title: 'Roller Shades',
    iconClass: 'nb-roller-shades',
    type: 'success',
  };
  wirelessAudioCard: CardSettings = {
    title: 'Wireless Audio',
    iconClass: 'nb-audio',
    type: 'info',
  };
  coffeeMakerCard: CardSettings = {
    title: 'Coffee Maker',
    iconClass: 'nb-coffee-maker',
    type: 'warning',
  };

  statusCards: string;

  commonStatusCardsSet: CardSettings[] = [
    this.lightCard,
    this.rollerShadesCard,
    this.wirelessAudioCard,
    this.coffeeMakerCard,
  ];

  statusCardsByThemes: {
    default: CardSettings[];
    cosmic: CardSettings[];
    corporate: CardSettings[];
  } = {
    default: this.commonStatusCardsSet,
    cosmic: this.commonStatusCardsSet,
    corporate: [
      {
        ...this.lightCard,
        type: 'warning',
      },
      {
        ...this.rollerShadesCard,
        type: 'primary',
      },
      {
        ...this.wirelessAudioCard,
        type: 'danger',
      },
      {
        ...this.coffeeMakerCard,
        type: 'secondary',
      },
    ],
  };

  constructor(
    private themeService: NbThemeService, 
    private _dashboardService: DashboardService,
    private _listService: ListService,
    private _datePipe: DatePipe,) 
  {
    
    this.themeService.getJsTheme()
      .pipe(takeWhile(() => this.alive))
      .subscribe(theme => {
        this.statusCards = this.statusCardsByThemes[theme.name];
    });
  }
  pickerDate(event: any) {
   this.start = this._datePipe.transform(new Date(event.start), "yyyy-MM-dd");
   this.end = this._datePipe.transform(new Date(event.end), "yyyy-MM-dd");
   this.searchModel.from = this.start;
   this.searchModel.to = this.end;    
    console.log(this.start);
    console.log(this.end);
    this.getListFilter();
    console.log(this.list);
  }//'T'HH:mm:ss.SSS'Z'

  pickerDate1(event: any){
    this.startDebt = this._datePipe.transform(new Date(event.start), "yyyy-MM-dd");
    this.endDebt = this._datePipe.transform(new Date(event.end), "yyyy-MM-dd");
   this.searchModel.from = this.startDebt;
   this.searchModel.to = this.endDebt;    
    console.log(this.startDebt);
    console.log(this.endDebt);
    this.getDebtFilter();
    this.getDebtTotal();
  }

  ngOnInit(){
    this.getListWarehouse();
    this.getDebtFilter();
    this.getDebtTotal();
  }
  getListStock(){
    this._dashboardService.getList()
      .subscribe(
        value => {
          if(value[0] != null ){
            this.list = value[0].StockList;
          }
          else{
            this.list = null;
          }
        },
        error => {
          console.log(<any>error)
        }
      );
  }
  getListDebt(){
    this._dashboardService.getReport()
    .subscribe(
      value => {
        this.debt = value.value;
        console.log(this.debt);
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
          this.listWarehouse = value.value;
        },
        error => {
          console.log(<any>error)
        }
      );
  }
  getListFilter(): void{
    this._dashboardService.getFilterList(this.searchModel)
    .subscribe(
      value => {
        console.log(value);
        if(value[0] != null ){
          this.list = value[0].StockList;
        }
        else{
          this.list = null;
        }
        //his.list = value[0].StockList;
      },
      error => {
        console.log(<any>error)
      }
    );
  }
  getDebtFilter(): void{
    this._dashboardService.getReportFilter(this.searchModel)
    .subscribe(
      value => {
        console.log(value);
      
        this.debt = value;
        console.log(this.debt);
      },
      error => {
        console.log(<any>error)
      }
    );
  }
  getDebtTotal(): void{
    this._dashboardService.getReportTotal(this.searchModel)
    .subscribe(
      value => {
        console.log(value);
      
        this.TongNo = value;
        console.log(this.TongNo);
      },
      error => {
        console.log(<any>error)
      }
    );
  }
  selectFromWarehouseHandler(event: any){
    this.searchModel.Id = event.target.value;
    this.getListFilter();
    console.log(this.list);
  }
  settings = {
    mode: 'external',
    hideSubHeader: true,
    actions: {
      add: false,
      delete: false,
      edit: false,
    },
    columns: {
      ProductName: {
        title: 'Sản phẩm',
        filter: false
      },
      OpeningStock: {
        title: 'Tồn đầu',
        filter: false
      },
      ImportQuantity: {
        title: 'Nhập',
        filter: false
      },
      ExportQuantity: {
        title: 'Xuất',
        filter: false
      },
      ClosingStock: {
        title: 'Tồn cuối',
        filter: false
      },
    }
  };

  debtReport = {
    mode: 'external',
    hideSubHeader: true,
    actions: {
      add: false,
      delete: false,
      edit: false,
    },
    columns: {
      WarehouseName: {
        title: 'Đại lý',
        filter: false
      },
      NoDauKy: {
        title: 'Đầu kỳ',
        filter: false,
        valuePrepareFunction: (value) => { return value === 'Total'? value : Intl.NumberFormat().format(value)}
      },
      NoCuoiKy: {
        title: 'Cuối kỳ',
        filter: false,
        valuePrepareFunction: (value) => { return value === 'Total'? value : Intl.NumberFormat().format(value)}
      },
    }
  };
}
