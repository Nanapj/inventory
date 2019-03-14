import { Component } from '@angular/core';

import { MENU_ITEMS } from './pages-menu';
import { CanActivate, Router } from '@angular/router';
import { Http } from '@angular/http';

@Component({
  selector: 'ngx-pages',
  template: `
    <ngx-sample-layout>
      <nb-menu [items]="menu"></nb-menu>
      <router-outlet></router-outlet>
    </ngx-sample-layout>
  `,
})
export class PagesComponent {

  constructor(private http: Http, private router: Router){
    var logged = localStorage.getItem("loggedIn");
    console.log(logged);
    if(logged == null){
      this.router.navigate(['/auth/login']);
    }else{
      this.router.navigate(['/page/dashboard']);
    }
  }
  menu = MENU_ITEMS;
}
