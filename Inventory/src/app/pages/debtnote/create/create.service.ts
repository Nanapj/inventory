import 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import {DebtNote} from '../base/debtnote';
import { CONFIG } from '../../../app.config';

@Injectable()
export class CreateService {
    constructor(public _http: Http) { }
    private BaseUrl = CONFIG.baseURL + '/odata/DebtNote';
    private PaymentTypeUrl = CONFIG.baseURL + '/odata/PaymentType';

    setExpand(expand: string) {
        this.BaseUrl + '?$expand=' + expand;
    }

    createDebtNote(debtnote: DebtNote): any {
        return this._http.post(this.BaseUrl, JSON.stringify(debtnote), new RequestOptions({ headers: CONFIG.headers }));
    }
    getListPayment(): any  {
        return this._http.get(this.PaymentTypeUrl)
            .map(res => res.json())
            .catch(this.handleError);
    }
    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}