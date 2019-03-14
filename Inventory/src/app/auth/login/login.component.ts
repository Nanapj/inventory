import { ChangeDetectionStrategy, Component } from '@angular/core';
import { User } from '../base/user';
import { LoginService } from './login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'nb-login',
  templateUrl: './login.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [LoginService]
})
export class LoginComponent {
  user = new User();

  constructor(private _loginService: LoginService, private router: Router) {
    localStorage.removeItem("loggedIn"); 
  }

  Login(): void{
    this._loginService.checkLogin(this.user).subscribe(
      data => {
        if(data.json().Success == true){
          localStorage.setItem("loggedIn", "true"); 
          this.router.navigate(['/page/dashboard']);
        }
      },
      error => {
          console.log("ERROR", error);
      }
    )
  }
}
