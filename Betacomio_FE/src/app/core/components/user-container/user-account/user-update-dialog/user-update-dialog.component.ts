import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ApiManagerService } from 'src/app/services/api-manager.service';

@Component({
  selector: 'app-user-update-dialog',
  templateUrl: './user-update-dialog.component.html',
  styleUrls: ['./user-update-dialog.component.css']
})
export class UserUpdateDialogComponent {

  middleName: string | undefined = this.api.loggedUser?.middleName
  phone: string | undefined = this.api.loggedUser?.phone
  userName: string | undefined = this.api.loggedUser?.userName
  firstName: string | undefined = this.api.loggedUser?.firstName
  lastName: string | undefined = this.api.loggedUser?.lastName
  emailAddress: string | undefined = this.api.loggedUser?.emailAddress
  userId: number | undefined = this.api.loggedUser?.userId

  constructor(public api: ApiManagerService, private dialog: MatDialog) {

  }

  updateUser(updateF: NgForm) {
    this.api.updateUser(updateF);
    this.dialog.closeAll();
  }
}
