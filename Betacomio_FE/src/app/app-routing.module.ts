import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ParentCategoriesComponent } from './core/components/parent-categories/parent-categories.component';
import { ProductModelsComponent } from './core/components/product-models/product-models.component';
import { ProductsComponent } from './core/components/products/products.component';
import { CartComponent } from './features/cart/cart.component';
import { RegistrationComponent } from './features/registration/registration.component';
import { AdminToolsComponent } from './core/components/adminComponents/admin-tools.component';
import { UserAccountComponent } from './core/components/user-container/user-account/user-account.component';
import { PlacedOrderComponent } from './features/cart/placed-order/placed-order.component';
import { UserHistoryComponent } from './core/components/user-container/user-history/user-history.component';
import { UserContainerComponent } from './core/components/user-container/user-container.component';

const routes: Routes = [  
  {path: 'parentCategories', component: ParentCategoriesComponent},
  {path: `productModels/:name`, component: ProductModelsComponent},
  {path: 'products/:modelName', component: ProductsComponent},
  {path: 'cart', component: CartComponent},
  {path: 'registration', component: RegistrationComponent},
  {path: 'admin', component: AdminToolsComponent},
  {path: 'user', component: UserAccountComponent},
  {path: 'placedOrder', component: PlacedOrderComponent},
  {path: 'history', component: UserHistoryComponent},
  {path: 'account', component: UserContainerComponent},
  {path: '**', redirectTo: 'parentCategories'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
