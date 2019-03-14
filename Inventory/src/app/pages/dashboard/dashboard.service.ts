import 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import { CONFIG } from '../../app.config';
import { Search } from '../warehouse/base/search';
import { serializePath } from '@angular/router/src/url_tree';
 
@Injectable()
export class DashboardService {
    constructor(public _http: Http) { }
    private BaseUrl = CONFIG.baseURL;

    setExpand(expand: string) {
        this.BaseUrl + '?$expand=' + expand;
    }
    getFilterList(search: Search): any {
        return this._http.get(this.BaseUrl + '/Json/getSLNhap?WarehouseId=' + search.Id  + '&Start=' + search.from + '&End=' + search.to )
            .map(res => res.json())
            .catch(this.handleError);
    }
    getFilterList1(search: Search): any {
        return this._http.get(this.BaseUrl + '/odata/Note/GetProductInStock?WarehouseId=' + search.Id)
            .map(res => res.json())
            .catch(this.handleError);
    }
    getList(): any  {
        return this._http.get(this.BaseUrl + '/odata/Note/GetProductInStock?WarehouseId=84981316-df02-4101-8f70-762f343d62c9' )
            .map(res => res.json())
            .catch(this.handleError);
    }
    getReport(): any  {
        return this._http.get(this.BaseUrl + '/odata/DebtNote/GetDebtInStock' )
            .map(res => res.json())
            .catch(this.handleError);
    }
    getReportFilter(search: Search): any {
        return this._http.get(this.BaseUrl + '/Json/getCongNo?Start=' + search.from + '&End=' + search.to )
            .map(res => res.json())
            .catch(this.handleError);
    }
    getReportTotal(search: Search): any {
        return this._http.get(this.BaseUrl + '/Json/getTongCongNo?End=' + search.to)
            .map(res => res.json())
            .catch(this.handleError);
    }
    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}