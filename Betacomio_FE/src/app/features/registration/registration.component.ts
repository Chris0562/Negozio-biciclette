import { Component } from '@angular/core';
import { Validators } from '@angular/forms';
import { LoginManagerService } from 'src/app/services/login-manager.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {

  middleName: string | null = null;
  phoneNumber: string | null = null;
  userName: string | null = null;
  firstName: string | null = null;
  lastName: string | null = null;
  email: string | null = null;
  password: string | null = null;

  // passwordValidator = this.validatePassword(this.password);
  isValidated: boolean | string = 'null';


  constructor(public srv: LoginManagerService) {
  }

  validatePassword() {
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,}$/; 
    console.log(this.isValidated)
    if (regex.test(this.password!)) {
      this.isValidated = true
      console.log(this.isValidated)
    }
    else {
      this.isValidated = false
      console.log(this.isValidated)
    } 
  }
}
