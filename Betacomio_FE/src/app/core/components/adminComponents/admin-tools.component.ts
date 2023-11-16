import { Component } from '@angular/core';
import { ApiManagerService } from 'src/app/services/api-manager.service';
import { LoginManagerService } from 'src/app/services/login-manager.service';

@Component({
  selector: 'app-admin-tools',
  templateUrl: './admin-tools.component.html',
  styleUrls: ['./admin-tools.component.css']
})
export class AdminToolsComponent {

  ciao: boolean = false;

  constructor(public usrv: ApiManagerService, private srv: LoginManagerService) { }

  show() {
    this.ciao = !this.ciao;
  }

  getUsers() {
    if (this.usrv.users.length > 0) {
      this.usrv.users = [];
    }
    else {
      this.usrv.getUsers()
    }
  }
}
