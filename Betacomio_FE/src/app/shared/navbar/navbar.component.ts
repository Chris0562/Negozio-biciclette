import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, map, startWith, tap } from 'rxjs';
import { ProductManagerComponent } from 'src/app/core/components/adminComponents/product-manager/product-manager.component';
import { LoginComponent } from 'src/app/features/login/login.component';
import { ProductModels } from 'src/app/models/productModels';
import { ApiManagerService } from 'src/app/services/api-manager.service';
import { CartManagerService } from 'src/app/services/cart-manager.service';
import { LoginManagerService } from 'src/app/services/login-manager.service';
import { SessionManagerService } from 'src/app/services/session-manager.service';


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
  




})
export class NavbarComponent implements OnInit{






  myControl = new FormControl('');
  filteredOptions: Observable<ProductModels[]> | undefined = undefined

  constructor(public srv: LoginManagerService,
              private route: ActivatedRoute,
              private router: Router,
              private session: SessionManagerService,
              private cart: CartManagerService,
              private api: ApiManagerService,
              private dialog: MatDialog) { }


  ngOnInit() {
    this.filteredOptions = this.myControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value || '')))
  }     
  
  private _filter(value: string): ProductModels[] {
    const filterValue = value.toLowerCase();
    return this.api.productModels.filter(productModel => this.containsWord(productModel.nomeModello, value));
  }

  containsWord(text: string, value: string) {
    text = text.toLowerCase();
    value = value.toString().toLowerCase();

    const searchWords = value.split(' ');

    return searchWords.every(word => text.includes(word));
  }

  navigateToProduct(event: any) {
    if (this.myControl.value != '') {
      this.router.navigateByUrl(`products/${event}`).then(() => {
        window.location.reload();
      });
      
    }
  }

  checkSession() {
    return sessionStorage.getItem('AuthBasic');
  }

  checkAdmin() {
    if (this.api.isAdmin == true) {
      return true
    } 
    return false
  }

  openLoginDialog() {
    this.dialog.open(LoginComponent)
  }

  logout() {
    if (confirm('Are you sure? stay a little longer please')) {
      this.srv.clearSession();  
      this.router.navigateByUrl('parentCategories');
      this.cart.toCart = [];
      this.cart.countedCart = [];
      this.cart.total = 0;
      this.session.stop();
      this.srv.headerOptions = this.srv.headerOptions.set(
        'Authorization',
        ''
      );
      this.api.users = [];
      this.api.loggedUser = null;
      this.api.address = null;
  }     
  }
}



