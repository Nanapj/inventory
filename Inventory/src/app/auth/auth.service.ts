import 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { CONFIG } from '../app.config';

 
@Injectable()
export class AuthService {
    constructor(public _http: Http) { }
    isAuthenticated(): boolean{
        if(localStorage.getItem("loggedIn") === null){
            return false;
        }
        return true;
    }
}