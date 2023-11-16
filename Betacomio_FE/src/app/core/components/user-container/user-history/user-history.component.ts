import { Component } from '@angular/core';
import { ApiManagerService } from 'src/app/services/api-manager.service';

@Component({
  selector: 'app-user-history',
  templateUrl: './user-history.component.html',
  styleUrls: ['./user-history.component.css']
})
export class UserHistoryComponent {

  constructor (public api: ApiManagerService) {
    if (this.api.loggedUser) {
       this.api.getOrderHistoryByCustomerId();
    }   
  }
}
