import { Component } from '@angular/core';
import { Product } from 'src/app/models/product';
import { CartManagerService } from 'src/app/services/cart-manager.service';
import { OrderManagerService } from 'src/app/services/order-manager.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent {   

  constructor(public cart: CartManagerService,
              public order: OrderManagerService, 
              ) {
    
  }

  removeOneFromCart(product: Product) {
    this.cart.toCart.splice(this.cart.toCart.indexOf(product), 1);
    this.cart.addToSession();
  }

  addOneToCart(product: Product) {
    this.cart.toCart.push(product);
    this.cart.addToSession();
    console.log(product)
  }

  //indexOf stranamente ritorna l ultima occorrenza, quindi facciamo il reverse dell array in modo che l ultima diventi la prima.. e da li elimanere Qty prodotti.ps richiamo addToSession 
  removeProducts(product: Product, qty: number) {     //per aggiornare pagina e riordinar array toCArt
    this.cart.toCart.splice(this.cart.toCart.reverse().indexOf(product, 0), qty);
    this.cart.addToSession();
  }

  clearCart() {
    this.cart.toCart = [];
    this.cart.addToSession();
  }

  ciao() {
    console.log(this.cart.countedCart)
    console.log(this.cart.toCart)
    console.log(this.cart.countedCart[0])
  }
}
