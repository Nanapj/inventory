import 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import { Warehouse } from '../base/warehouse';
import { CONFIG } from '../../../app.config';
import { Search } from '../base/search';
 
@Injectable()
export class ListService {
    constructor(public _http: Http) { }
    private BaseUrl = CONFIG.baseURL + '/odata/Warehouse';

    setExpand(expand: string) {
        this.BaseUrl + '?$expand=' + expand;
    }

    getList(): any  {
        return this._http.get(this.BaseUrl)
            .map(res => res.json())
            .catch(this.handleError);
    }
    getListFilter(search: Search): any {
        return this._http.get(this.BaseUrl + '?Id=' + search.Id)
            .map(res => res.json())
            .catch(this.handleError);
    }

    createWarehouse(warehouse: Warehouse): any {
        return this._http.post(this.BaseUrl, JSON.stringify(warehouse), new RequestOptions({ headers: CONFIG.headers }));
    }

    deleteWarehouse(Id: string) : any{
        console.log("Delete " + Id);
        return this._http.delete(this.BaseUrl + '(' + Id + ')');
    }

    updateWarehouse(warehouse: Warehouse): any {
        return this._http.put(this.BaseUrl + '(' + warehouse.Id + ')', JSON.stringify(warehouse), new RequestOptions({ headers: CONFIG.headers }));
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}