import { ChangeDetectionStrategy, Component } from '@angular/core';
import {User} from '../base/user';
import {RegisterService} from './register.service';

@Component({
  selector: 'nb-register',
  styleUrls: ['./register.component.scss'],
  templateUrl: './register.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [RegisterService]
})

export class RegisterComponent {
  user = new User();

  constructor(private _registerService: RegisterService) {
  }

  Register(): void{
    console.log(this.user);
    this._registerService.createUser(this.user).subscribe(
      data => {
          console.log("POST request is success!", data);
          this.user = new User();
      },
      error => {
          console.log("ERROR", error);
      }
    );
  }
}
