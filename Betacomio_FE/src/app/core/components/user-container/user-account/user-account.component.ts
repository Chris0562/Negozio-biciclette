import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ApiManagerService } from 'src/app/services/api-manager.service';
import { AddressUpdateDialogComponent } from './address-update-dialog/address-update-dialog.component';
import { UserUpdateDialogComponent } from './user-update-dialog/user-update-dialog.component';

@Component({
  selector: 'app-user-account',
  templateUrl: './user-account.component.html',
  styleUrls: ['./user-account.component.css']
})
export class UserAccountComponent implements OnInit{

  constructor(public api: ApiManagerService, public dialog: MatDialog) {
    
  }

  ngOnInit() {
    this.api.getAddress();
  }

  updateAddress() {    
    this.dialog.open(AddressUpdateDialogComponent);
  }

  

  updateUser() {    
    this.dialog.open(UserUpdateDialogComponent);
  }
}
