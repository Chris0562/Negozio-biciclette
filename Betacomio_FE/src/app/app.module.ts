import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { FooterComponent } from './shared/footer/footer.component';
import { ParentCategoriesComponent } from './core/components/parent-categories/parent-categories.component';
import { CategoriesComponent } from './core/components/categories/categories.component';
import { ProductModelsComponent } from './core/components/product-models/product-models.component';
import { ProductsComponent } from './core/components/products/products.component';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { CartComponent } from './features/cart/cart.component';
import { LoginComponent } from './features/login/login.component';
import { RegistrationComponent } from './features/registration/registration.component';
import { UserManagerComponent } from './core/components/adminComponents/user-manager/user-manager.component';
import { ProductManagerComponent } from './core/components/adminComponents/product-manager/product-manager.component';
import { AdminToolsComponent } from './core/components/adminComponents/admin-tools.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog'; 
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { NgIdleKeepaliveModule } from '@ng-idle/keepAlive';
import { UserAccountComponent } from './core/components/user-container/user-account/user-account.component';
import { AddressUpdateDialogComponent } from './core/components/user-container/user-account/address-update-dialog/address-update-dialog.component';
import { PlacedOrderComponent } from './features/cart/placed-order/placed-order.component';
import { UserHistoryComponent } from './core/components/user-container/user-history/user-history.component';
import { UserUpdateDialogComponent } from './core/components/user-container/user-account/user-update-dialog/user-update-dialog.component';
import { UserContainerComponent } from './core/components/user-container/user-container.component'
import {MatTabsModule} from '@angular/material/tabs';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    ParentCategoriesComponent,
    CategoriesComponent,
    ProductModelsComponent,
    ProductsComponent,
    CartComponent,
    LoginComponent,
    RegistrationComponent,
    AdminToolsComponent,
    UserManagerComponent,
    ProductManagerComponent,
    UserAccountComponent,
    AddressUpdateDialogComponent,
    PlacedOrderComponent,
    UserHistoryComponent,
    UserUpdateDialogComponent,
    UserContainerComponent,
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatCheckboxModule,
    MatPaginatorModule,
    MatTableModule,
    MatDialogModule,
    MatButtonModule,
    MatExpansionModule,
    MatAutocompleteModule,
    NgIdleKeepaliveModule.forRoot(),
    MatButtonModule,
    MatTabsModule
  ],
  providers: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
