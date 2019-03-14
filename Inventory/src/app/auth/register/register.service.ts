import 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import {User } from '../base/user';
import { CONFIG } from '../../app.config';

@Injectable()
export class RegisterService {
    constructor(public _http: Http) { }
    private BaseUrl = CONFIG.baseURL + '/register';

    setExpand(expand: string) {
        this.BaseUrl + '?$expand=' + expand;
    }

    createUser(user: User): any {
        return this._http.post(this.BaseUrl, JSON.stringify(user), new RequestOptions({ headers: CONFIG.headers }));
    }
   
    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}