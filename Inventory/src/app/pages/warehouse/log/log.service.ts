import 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import { CONFIG } from '../../../app.config';
import { Search } from '../base/search';
@Injectable()
export class LogService {
    constructor(public _http: Http) { }
    private BaseUrl = CONFIG.baseURL + '/odata/Note';
    private ExportUrl = CONFIG.baseURL + '/Excel/DeNoteExcel';
    private ImportUrl = CONFIG.baseURL + '/Excel/ReNoteExcel';
    
    setExpand(expand: string) {
        this.BaseUrl + '?$expand=' + expand;
    }

    filterNote(search: Search): any {
        return this._http.get(this.BaseUrl +'?Id=' + search.Id + '&Code=' + search.Code + '&SourceId=' + search.SourceId + '&DestinationId=' + search.DestinationId + '&from=' + search.from + '&to=' + search.to)
            .map(res => res.json())
            .catch(this.handleError);
    }
    downloadDeliverNote(search: Search): any{
        var wnd = window.open(this.ExportUrl + '?Id=' + search.Id );
        setTimeout(function() {
            wnd.close();
          }, 3000);
          return false;
       
    }
    downloadReceiveNote(search: Search): any{
        var wnd = window.open(this.ImportUrl + '?Id=' + search.Id );
        setTimeout(function() {
            wnd.close();
          }, 3000);
          return false;
    }
    deleteLog(Id: string) : any{
        console.log("Delete " + Id);
        return this._http.delete(this.BaseUrl + '(' + Id + ')');
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