import 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import { Product } from '../base/product';
import { CONFIG } from '../../../app.config';
 
@Injectable()
export class ProductService {
    constructor(public _http: Http) { }
    private BaseUrl = CONFIG.baseURL + '/odata/Product';

    setExpand(expand: string) {
        this.BaseUrl + '?$expand=' + expand;
    }

    getList(): any  {
        return this._http.get(this.BaseUrl)
            .map(res => res.json())
            .catch(this.handleError);
    }

    createProduct(product: Product): any {
        return this._http.post(this.BaseUrl, JSON.stringify(product), new RequestOptions({ headers: CONFIG.headers }));
    }

    deleteProduct(Id: string) : any{
        console.log("Delete " + Id);
        return this._http.delete(this.BaseUrl + '(' + Id + ')');
    }

    updateProduct(product: Product): any {
        return this._http.put(this.BaseUrl + '(' + product.Id + ')', JSON.stringify(product), new RequestOptions({ headers: CONFIG.headers }));
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}