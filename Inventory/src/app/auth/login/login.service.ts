import 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import { CONFIG } from '../../app.config';
import {User} from '../base/user';
 
@Injectable()
export class LoginService {
    constructor(public _http: Http) { }
    private BaseUrl = CONFIG.baseURL + '/login';
    private TokenUrl = CONFIG.baseURL + '/Token';

    checkLogin(user: User): any{
        return this._http.post(this.BaseUrl, JSON.stringify(user), new RequestOptions({ headers: CONFIG.headers }));
    }
    
    token(user: User): any{
        return this._http.post(this.TokenUrl, JSON.stringify(user), new RequestOptions({ headers: CONFIG.TokenHeaders }));
    }
}