import { Injectable } from '@angular/core';
import { Product } from '../models/product';
import { ApiManagerService } from './api-manager.service';
import { HttpClient, HttpStatusCode } from '@angular/common/http';
import { CountedCart } from '../models/countedCart';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class CartManagerService {

  sortedCart: Product[] = [];
  counter: number[] = [];
  countedCart: CountedCart[] = [];
  toCart: Product[] = [];
  userCart: UserCart | null =  null;
  total: number = 0;
  loggedUserCopy: User | null = null;

  constructor(private http: HttpClient) {
                
    
  }

  
  getCartByUserID(loggedUser: User) {
    this.loggedUserCopy = loggedUser;
    this.http.get<UserCart>(`https://localhost:7081/api/Carts/${loggedUser.userId}`)
      .subscribe(result => {
        this.toCart = JSON.parse(atob(result.cartItems));
        this.setSortedCart();
      })
  }

  
  setSortedCart() {  

    this.toCart.sort((a, b) => (a.productId < b.productId ? -1 : 1));
    
    //setta array vuoti
    this.sortedCart = [];
    this.counter = [];
    this.countedCart = [];
    this.total = 0;
    
    //crea copia di toCart
    for (let i = 0; i < this.toCart.length; i++) {
      this.sortedCart.push(this.toCart[i]);
    }

    //crea counter e product e pusha in CountedCart
    for (let i = 0; i < this.sortedCart.length; i++) {
      this.counter[i] = 1;
      while (this.sortedCart[i]?.productId === this.sortedCart[i + 1]?.productId) {
        this.sortedCart.splice(i, 1);
        this.counter[i] += 1;
        
      }
      this.countedCart.push({product:this.sortedCart[i], orderQty: this.counter[i]})
    }

    //calcola totale carrello
    for (let x of this.countedCart) {
      var u = x.orderQty * x.product.standardCost
      this.total = this.total + u;
    
    }
    
  }


  addToSession() {
    sessionStorage.setItem('cart', window.btoa(JSON.stringify(this.toCart)));  //salvo carrello stringhifato nella session
    this.postCart();
  }

  postCart() {
    this.userCart =  {  //popolo carrello da mandare a BE
      userID: this.loggedUserCopy!.userId,  //utilizzare logged user se possibile
      cartItems: sessionStorage.getItem('cart')!  //prendo carrello dalla session
    }
    this.http.post('https://localhost:7081/api/Carts', this.userCart)
       .subscribe(() => {
          console.log(this.loggedUserCopy)
          this.getCartByUserID(this.loggedUserCopy!)  //recupero il carrello aggiornato
         
       });
  }
}

export interface UserCart {
  userID: number,
  cartItems: string
}
