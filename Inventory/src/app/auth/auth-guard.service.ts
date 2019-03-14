import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Http } from '@angular/http';
@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private http: Http) {
  }

  canActivate() {
    var logged = localStorage.getItem("loggedIn");
    console.log(logged);
    if(logged == null){
      return this.router.navigate(['/auth/login']);
    }else{
      return this.router.navigate(['/page/dashboard']);
    }
  }
}