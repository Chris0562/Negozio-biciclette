import { HttpStatusCode } from '@angular/common/http';
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { ApiManagerService } from 'src/app/services/api-manager.service';
import { LoginManagerService } from 'src/app/services/login-manager.service';
import { SessionManagerService } from 'src/app/services/session-manager.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  email: string | null = null;
  password: string | null = null;
  
  
  constructor(public srv: LoginManagerService,
              private api: ApiManagerService,
              private router: Router,
              private session: SessionManagerService,
              private dialog: MatDialog) {}

  login(loginF : NgForm) {
    this.api.cred = loginF.value
    this.srv.login(this.api.cred!).pipe(
      catchError(error => {
        console.error('errore', error)
        switch (error.status) {
          case HttpStatusCode.InternalServerError:
            if (confirm('Account is too old, or inexistent, Create a new account.\nYou can use your old eMail to mantain your history')) {
              this.router.navigateByUrl('registration');
              this.dialog.closeAll();
            };
            break;
          case HttpStatusCode.BadRequest:
            alert('REQUEST NOT VALID !!!');
            break;
          case HttpStatusCode.RequestTimeout:
            alert('SESSION EXPIRED!!')
            break;
          default:
            alert('Errore sconosciuto.');
            break;
        }
        return throwError(() => new Error('Error'))
      })).subscribe(resp => {
          if (resp.status === HttpStatusCode.Ok) {
              this.srv.setAuthToken(this.api.cred!);
              this.api.getUserByEmail(this.api.cred!.email);
              this.api.getAddress();
              this.dialog.closeAll();
              this.session.reset();
          }
        })
  }
}

export interface Credentials {
  email: string;
  password: string;  
}

