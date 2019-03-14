import 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import { CONFIG } from '../../../app.config';
import { Search } from '../base/search';
@Injectable()
export class PrintService {
    constructor(public _http: Http) { }
    private BaseUrl = CONFIG.baseURL + '/odata/NoteDetail';

    setExpand(expand: string) {
        this.BaseUrl + '?$expand=' + expand;
    }

    filterNote(search: Search): any {
        return this._http.get(this.BaseUrl + '?SourceId=' + search.SourceId + '&DestinationId=' + search.DestinationId)
            .map(res => res.json())
            .catch(this.handleError);
    }
    
    getList(): any  {
        return this._http.get(this.BaseUrl)
            .map(res => res.json())
            .catch(this.handleError);
    }
    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}