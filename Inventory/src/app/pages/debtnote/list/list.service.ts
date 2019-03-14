import 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import { DebtNote } from '../base/debtnote';
import { CONFIG } from '../../../app.config';
import { Search } from '../../warehouse/base/search';
 
@Injectable()
export class ListService {
    constructor(public _http: Http) { }
    private BaseUrl = CONFIG.baseURL + '/odata/DebtNote';

    setExpand(expand: string) {
        this.BaseUrl + '?$expand=' + expand;
    }

    getList(): any  {
        return this._http.get(this.BaseUrl)
            .map(res => res.json())
            .catch(this.handleError);
    }
    getListCollect(): any  {
        return this._http.get(this.BaseUrl + '?$filter=isOwed eq false' )
            .map(res => res.json())
            .catch(this.handleError);
    }
    getListDebit (): any  {
        return this._http.get(this.BaseUrl + '?$filter=isOwed eq true' )
            .map(res => res.json())
            .catch(this.handleError);
    }
    getListDebitFilter(search: Search): any {
        return this._http.get(this.BaseUrl + '?$filter=isOwed eq true' + '&from=' + search.from + '&to=' + search.to)
            .map(res => res.json())
            .catch(this.handleError);
    }
    getListCollectFilter(search: Search): any {
        return this._http.get(this.BaseUrl + '?$filter=isOwed eq false' + '&from=' + search.from + '&to=' + search.to)
            .map(res => res.json())
            .catch(this.handleError);
    }
    createDebtNote(debtnote: DebtNote): any {
        return this._http.post(this.BaseUrl, JSON.stringify(debtnote), new RequestOptions({ headers: CONFIG.headers }));
    }

    deleteDebtNote(Id: string) : any{
        console.log("Delete " + Id);
        return this._http.delete(this.BaseUrl + '(' + Id + ')');
    }

    updateDebtNote(debtnote: DebtNote): any {
        return this._http.put(this.BaseUrl + '(' + debtnote.Id + ')', JSON.stringify(debtnote), new RequestOptions({ headers: CONFIG.headers }));
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}