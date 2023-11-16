import { Injectable } from '@angular/core';
import { ApiManagerService } from './api-manager.service';
import { CartManagerService } from './cart-manager.service';
import { HttpClient, HttpStatusCode } from '@angular/common/http';
import { OrderInfo } from '../models/orderInterfaces';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class OrderManagerService {

  orderInfo: OrderInfo | null = null;
  

  constructor(private api: ApiManagerService,
              private cart: CartManagerService,
              private http: HttpClient,
              private router: Router) { }

  purchaseCart() {
    if (this.cart.countedCart.length > 0) {
      var date = new Date()
      date.setDate(date.getDate() + 7)
      this.orderInfo = {
        dueDate: date,
        customerId: this.api.loggedUser!.oldCustomerId2 ? this.api.loggedUser!.oldCustomerId2 : this.api.loggedUser!.userId,
        shipAddressId: this.api.address!.address.addressId,
        billToAddressId: this.api.address!.address.addressId,
        shipMethod: 'CARGO TRANSPORT 5',
        subTotal: this.cart.total,
        details: this.cart.countedCart
      }    
      console.log(this.orderInfo)
      this.http.post<any>('https://localhost:7081/api/SalesOrderHeaders', this.orderInfo) //ricordare token header user
        .subscribe(() => {
          if (HttpStatusCode.Ok) {
            this.http.delete<any>(`https://localhost:7081/api/Carts/${this.api.loggedUser?.userId}`)
                .subscribe(() => {
                  if (HttpStatusCode.NoContent) {
                    this.cart.toCart = [];
                    
                    this.cart.sortedCart = [];
                    this.cart.counter = [];
                    this.cart.total = 0;
                    this.router.navigateByUrl('placedOrder')
                  }            
                })
          }
          else if (HttpStatusCode.BadRequest) {
            alert('REQUEST NOT VALID !!!');
          }
          else if (HttpStatusCode.RequestTimeout) {
            alert('SESSION EXPIRED!!')
          }     
        })
    }
    
  }
}
