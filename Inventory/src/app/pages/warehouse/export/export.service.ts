import 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import {Note} from '../../warehouse/base/note';
import { CONFIG } from '../../../app.config';

@Injectable()
export class ExportService {
    constructor(public _http: Http) { }
    private BaseUrl = CONFIG.baseURL + '/odata/Note';

    setExpand(expand: string) {
        this.BaseUrl + '?$expand=' + expand;
    }

    createNote(note: Note): any {
        return this._http.post(this.BaseUrl, JSON.stringify(note), new RequestOptions({ headers: CONFIG.headers }));
    }
   
    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}