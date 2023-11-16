import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ApiManagerService } from 'src/app/services/api-manager.service';

@Component({
  selector: 'app-address-update-dialog',
  templateUrl: './address-update-dialog.component.html',
  styleUrls: ['./address-update-dialog.component.css']
})
export class AddressUpdateDialogComponent {

  addressLine1: string | undefined = this.api.address?.address?.addressLine1;
  addressLine2: string | undefined = this.api.address?.address?.addressLine2;
  city: string | undefined = this.api.address?.address?.city;
  countryRegion: string | undefined = this.api.address?.address?.countryRegion;
  stateProvince: string | undefined = this.api.address?.address?.stateProvince;
  postalCode: string | undefined = this.api.address?.address?.postalCode;
  addressType: string | undefined = this.api.address?.addressType;

  constructor(public api: ApiManagerService, public dialog: MatDialog) {
    
  }

  submitAddress(updateAF: NgForm) {
    this.api.submitAddress(updateAF);
    this.dialog.closeAll();
  }

  
}
