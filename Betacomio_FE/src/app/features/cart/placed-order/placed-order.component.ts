import { Component } from '@angular/core';
import { ApiManagerService } from 'src/app/services/api-manager.service';
import { CartManagerService } from 'src/app/services/cart-manager.service';

@Component({
  selector: 'app-placed-order',
  templateUrl: './placed-order.component.html',
  styleUrls: ['./placed-order.component.css']
})
export class PlacedOrderComponent {

  constructor (public api: ApiManagerService, public cart: CartManagerService) {
    this.api.getLastPlacedOrder();
  }

  ngOnDestroy() {
    this.cart.countedCart = [];
  }
}
