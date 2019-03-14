import { Headers } from "@angular/http";

export const CONFIG = {
    //baseURL: 'http://localhost:61119',
    baseURL: '',
    headers: new Headers({ 
        'Content-Type': 'application/json; charset=urf-8',
        'Access-Control-Allow-Credential': 'true',
        'Allow-Control-Allow-Origin': '*',
        'Accept': 'application/json',
        'dataType': 'json'
    }),
    TokenHeaders: new Headers({ 
        'Content-Type': 'application/x-www-form-urlencoded'
    }),
};