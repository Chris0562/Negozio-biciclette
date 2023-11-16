import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Credentials, LoginComponent } from '../features/login/login.component';
import { MatDialog } from '@angular/material/dialog';
import { tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class LoginManagerService {

  userID: any;
  headerOptions = new HttpHeaders().set('Content-Type', 'application/json');   

  constructor(private http: HttpClient,
              private dialog: MatDialog,
              private router: Router) {

                // popola headerOptions in caso di riavvio pagina
    if (this.headerOptions && sessionStorage.length > 0) {
      this.headerOptions = this.headerOptions.set(
      'Authorization',
      sessionStorage.getItem('AuthBasic')!
    );     
    }
    
   }

  login(cred : Credentials) {
    return this.http.post('https://localhost:7081/api/Users/login', cred, {
      observe: 'response'
    });
  }

  setAuthToken(cred : Credentials) {
    sessionStorage.setItem('AuthBasic', 'Basic ' + window.btoa(cred.email + ':' + cred.password))

    this.headerOptions = this.headerOptions.set(
      'Authorization',
      sessionStorage.getItem('AuthBasic')!
    );    
  }

  clearSession() {
    sessionStorage.clear();
  }



  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


  postNewUser(f: NgForm) {
    this.http.post('https://localhost:7081/api/Users', f.value)
      .subscribe(() => {
        this.dialog.open(LoginComponent).afterClosed().pipe(   ///afterClose è un Observable, quindi richiede subscribe.
          tap(() => {
            console.log('sono partito da register')
            if (sessionStorage.length > 0) this.router.navigateByUrl('/parentCategories')
          })
        ).subscribe();
      })
      
  }

   postNewAdmin(adminF: NgForm) {
    console.log(adminF.value)
    this.http.post('https://localhost:7081/api/Users', adminF.value)
      .subscribe(result => console.log(result))
  }
}
