import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, NavigationEnd, NavigationStart, Router } from '@angular/router';
import { Subscription, first, take, tap } from 'rxjs';
import { LoginComponent } from 'src/app/features/login/login.component';
import { ColorSize } from 'src/app/models/colorSize';
import { ApiManagerService } from 'src/app/services/api-manager.service';
import { CartManagerService } from 'src/app/services/cart-manager.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent {

  modelName: string = '';
  colorSize: ColorSize | null = null;

  constructor(private route: ActivatedRoute,
              private router: Router,
              public srv: ApiManagerService,
              private csrv: CartManagerService,
              public dialog: MatDialog) {
    this.modelName = this.route.snapshot.params['modelName']
    this.srv.getProducts(this.modelName)
  }

  addToCart(f: NgForm) {
    if (sessionStorage.length > 0) {  //controlla se sei loggato
      this.colorSize = f.value; //prende valore dal form colore/taglia
      this.srv.products.forEach( product => {   //srv.product è stato popolato nel costruttore con tutti i prodotti del modello selezionato, per ognuno di essi controllo: 
        if (product.color === this.colorSize?.color && product.size === this.colorSize?.size) {  //se il colore del prodotto e la taglia sono uguali a quello selezionato e pusha nel carrello
          this.csrv.toCart.push(product);
          } else if (product.size === null && product.color === null) {
            this.csrv.toCart.push(product);
          } else if (product.color === this.colorSize?.color && product.size === null) {
            this.csrv.toCart.push(product);
          } else if (product.color === null && product.size === this.colorSize?.size) {
            this.csrv.toCart.push(product);
          }
      });  
      this.csrv.addToSession() ;  //manda al servizio che si occupa di postare il carrello
    } else {   //se non sei loggato apre dialog login
      this.dialog.open(LoginComponent) 
        .afterClosed()
          .pipe(                                  //afterClosed vuole un Observable, quindi utilizziamo .pipe per effettuare operazioni al suo interno
            tap(() => {   
              if(this.srv.loggedUser) {   //richiamo addToCart solo se il login è andato a buon fine e completo la subscribe, il metodo riparte entra nel primo if
                this.addToCart(f);
              }                   
            })).subscribe();
    }    
  }

  selectWeight(size: any) {
    
    this.srv.products.forEach( product => {  //cambia il peso in base alla taglia selezionata
      if (size === product.size) {
        this.srv.weight = product.weight
      }      
    }) 
  }
}