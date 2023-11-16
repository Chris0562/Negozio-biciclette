import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ParentCategories } from '../models/parentCategories';
import { Categories } from '../models/categories';
import { ProductModels } from '../models/productModels';
import { Product } from '../models/product';
import { User } from '../models/user';
import { NgForm } from '@angular/forms';
import { LoginManagerService } from './login-manager.service';
import { Address, AddressForm, CustomerAddress, GetAddress, GetCustomerAddress, PostCustomerAddress } from '../models/customerAddress';
import { OrderHistory } from '../models/orderHistory';
import { Credentials } from '../features/login/login.component';
import { CartManagerService } from './cart-manager.service';
import { MatDialog } from '@angular/material/dialog';
import { first } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiManagerService {

  parentCategories: ParentCategories[] | null = null;
  categories: Categories[] | null = null;
  parentCategoryName: string | null = null;
  singleCategories: Categories[] | null = null;
  productModels: ProductModels[] = [];
  products: Product[] = [];
  adminProducts: Product[] = [];

  uniqueColors : string[] = [];
  uniqueSize : any[] = [];
  weight: any | null = null;

  
  cred: Credentials | null = null;
  users: User[] = [];
  user : User | null = null;
  userToAdmin: User | null = null;
  uID: number | null = null;
  loggedUser: User | null = null; 
  updateForm: User | null = null;  
  isAdmin: boolean = false;

  address: CustomerAddress | null = null;    /// prevedere Address[] in caso di indirizzxi multipli per singolo utente
  addressToAddress: Address | null = null;
  addressToCustomerAddress: PostCustomerAddress | null = null;
  formAddress: AddressForm | null = null;
  updateAddressForm: GetAddress | null = null;
  updateCustomerAddressForm: GetCustomerAddress | null = null;

  orderHistory: OrderHistory[] | null = null;
  lastPlacedOrder: OrderHistory | null = null;
  

  constructor(private http: HttpClient,
              private srv: LoginManagerService,
              private cart: CartManagerService,) { 
                this.getParentCategories();
                this.getAllCategories();  // carichaimo i prodotti nel costruttore del servizio per averli sempre disponibili per l'admin, anche in caso di aggiornamento web
                                         // utilizzate per select aggiungi nuovo prodotto
                this.getAllModels();      // i servizi vengono inizializzati quando viene creato il modulo
                this.getMailFromSession();
                if (sessionStorage.length > 0) {
                  this.getAddress();
                } 
              }

  getMailFromSession() {
    if (sessionStorage.length > 0) {
      let token = JSON.stringify(atob((sessionStorage.getItem('AuthBasic')!.substring(6))));
      var tempMail = token.split(':');
      this.getUserByEmail(tempMail[0].substring(1))
      console.log(this.loggedUser + 'ciao')
    }
    
  }

  ////////////////////////////////////////////////////////////////////// GET PRODUCTS ///////////////////////////////////////////////////////////////////////////////////////
  getParentCategories() {
    this.http.get<ParentCategories[]>('https://localhost:7081/api/ViewParentCategories')
      .subscribe(result => {
        this.parentCategories = result;
        console.log('ciao')
      });
  }

  getCategories(id: number) {
    this.http.get<Categories[]>(`https://localhost:7081/api/ProductCategories/${id}`)
     .subscribe(result => this.singleCategories = result)
  }

  /* prende categorie per admin, nella tabella categorie ci sono anche le 4 macro categorie,
  andrebbero filtrate dall array confrontandolo con l'array delle macro categorie popolato con getParentCategories riga 80*/
  getAllCategories() {
    this.http.get<Categories[]>(`https://localhost:7081/api/ProductCategories`)
     .subscribe(result => this.categories = result)  
  }

  getAllModels() {
    this.http.get<ProductModels[]>(`https://localhost:7081/api/ViewModels`)
      .subscribe(result => {
        this.productModels = result
      })      
  }

  getProductModels(name: string) {
    this.http.get<ProductModels[]>(`https://localhost:7081/api/ViewModels/modelsName/${name}`)
      .subscribe(result => {
        this.productModels = result;
      });
  }

  getProductModelsById(id: number) {  //usato nella select dell admin aggiungi prodotto
    this.http.get<ProductModels[]>(`https://localhost:7081/api/ViewModels/modelsID/${id}`)
      .subscribe(result => {
        this.productModels = result;
      });
  }

  getProductsAdmin(modelName: string) {
    
    this.http.get<Product[]>(`https://localhost:7081/api/ViewProductsOverview/${modelName}`)
    // this.http.get<Product[]>(`https://localhost:7081/api/ViewProductsOverview/HLMountain Seat%2FSaddle 2`)
      .subscribe(result => this.adminProducts = result)
  }

  getProducts(modelName: string) {
    this.http.get<Product[]>(`https://localhost:7081/api/ViewProductsOverview/${modelName}`)
      .subscribe(result => {
        this.products = result;
        this.uniqueColors = [];
        this.uniqueSize = [];
        this.weight = null;
        this.products.forEach(product => {
          if (this.uniqueColors.indexOf(product.color) === -1) {
            this.uniqueColors.push(product.color)
          }
          if (this.uniqueColors[0] === null) {
            this.uniqueColors = []
          }         
        });
        this.products.forEach(product => {
          if (this.uniqueSize.indexOf(product.size) === -1) {
            this.uniqueSize.push(product.size)
          }
          if (this.uniqueSize[0] === null) {
            this.uniqueSize = []
          }
        });
        if (this.uniqueSize.length === 0) {
          this.weight = this.products[0].weight
        }
      })
  }

  getAllProducts() {
    this.http.get<Product[]>('https://localhost:7081/api/ViewProductsOverview')
      .subscribe(result => this.products = result)
  }

  ////////////////////////////////////////////////////////////////////// POST/DELETE PRODUCTS ///////////////////////////////////////////////////////////////////////////////////////

  postNewProduct(productF: NgForm) {
    this.http.post('https://localhost:7081/api/Products', productF.value, {headers: this.srv.headerOptions})
      .subscribe(result => console.log(result))
  }

  deleteProduct(productID: number){ 
    if (confirm(`are you sure to delete: ${productID}`)) { 
      this.http.delete(`https://localhost:7081/api/Products/${productID}`, {headers: this.srv.headerOptions}) 
        .subscribe(result => console.log(result)); 
    } 
  }


  ////////////////////////////////////////////////////////////////////// USERS ///////////////////////////////////////////////////////////////////////////////
  
  getUsers() {
    console.log(this.srv.headerOptions)
    this.http.get<User[]>('https://localhost:7081/api/Users', {headers: this.srv.headerOptions})
      .subscribe(result => this.users = result);       
  }

  getUserById(userId : number) { //probibilmente non utilizzato...sostituito da getUserByEmail che popola loggedUser
    this.uID = userId;
    this.http.get<User>(`https://localhost:7081/api/Users/${userId}`)
      .subscribe(result => this.user = result);
  }

  getUserByEmail(email : string) {
    this.http.get<User>(`https://localhost:7081/api/Users/ByEmail/${email}`)
      .subscribe(result => {
        this.loggedUser = result;
        this.isAdmin = this.loggedUser.isAdmin;
        console.log(this.isAdmin)
        //  sessionStorage.setItem('isAdmin', this.loggedUser.isAdmin.toString())
        this.cart.getCartByUserID(this.loggedUser);
        this.getOrderHistoryByCustomerId();
      });
  }

  deleteUser(userId: number){
    if(confirm(`are you sure to delete: ${userId}`)){
    this.http.delete(`https://localhost:7081/api/Users/${userId}`, {headers: this.srv.headerOptions})
      .subscribe( () => console.log('deletato'))}
  }

  updateUser(updateF: NgForm) {    
    this.updateForm = {
      ...updateF.value,
      userId: this.loggedUser?.userId      
    }
    console.log(this.updateForm)
    console.log(updateF.value)
    this.http.put(`https://localhost:7081/api/Users/${this.loggedUser!.userId}`, this.updateForm)
      .subscribe(result => {
        console.log(result)
      })
        
  }



   ////////////////////////////////////////////////////////////////////// ADDRESS ///////////////////////////////////////////////////////////////////////////////


//#region 'lasciate ogni speranza o voi che entrate'
   getAddress() {  //if dentro subscribe per raspettare che riempa loggedUser
    this.address = null;

      if(this.loggedUser == null) {  // logged user va popolato nel costruttore del servizio una volta sola...non ogni volta che uso un metodo
        let token = JSON.stringify(atob((sessionStorage.getItem('AuthBasic')!.substring(6))));
        var tempMail = token.split(':');
        this.http.get<User>(`https://localhost:7081/api/Users/ByEmail/${tempMail[0].substring(1)}`)
          .subscribe(result => {
            this.loggedUser = result;

            if(this.loggedUser!.oldCustomerId2) { 
              this.http.get<CustomerAddress>(`https://localhost:7081/api/CustomerAddresses/${this.loggedUser?.oldCustomerId2}`) // non servono due get, uso ternario nella prima get, if inutile quindi
                .subscribe(result => this.address = result)
            }
            else {
              this.http.get<CustomerAddress>(`https://localhost:7081/api/CustomerAddresses/${this.loggedUser?.userId}`)
                .subscribe(result => this.address = result)
            }
          })        
      }
      else {
        if(this.loggedUser!.oldCustomerId2) {
          this.http.get<CustomerAddress>(`https://localhost:7081/api/CustomerAddresses/${this.loggedUser?.oldCustomerId2}`) // codice duplica..si potrebbe chiudere in un metodo...anche qui ternario
            .subscribe(result => this.address = result)
        }
        else {
          this.http.get<CustomerAddress>(`https://localhost:7081/api/CustomerAddresses/${this.loggedUser?.userId}`)
            .subscribe(result => this.address = result)
        }
         
    }  
  }

  submitAddress(updateAF: NgForm) {
    this.address == null ? this.postAddress(updateAF) : this.updateAddress(updateAF);  //ternario con due metodi...wow
  }

  postAddress(updateAF: NgForm) {  // non passare NgForm.. al submit passare(updateAF.value) e nel servizio prendere (updateAF : interfaccia custom)
    this.formAddress = updateAF.value;

      this.addressToAddress = {
        addressId: 0,
        addressLine1: this.formAddress!.addressLine1,
        addressLine2: this.formAddress!.addressLine2,
        city: this.formAddress!.city,  
        countryRegion: this.formAddress!.countryRegion,
        stateProvince: this.formAddress!.stateProvince,
        postalCode: this.formAddress!.postalCode
      }

      this.http.post<any>('https://localhost:7081/api/Addresses', this.addressToAddress)  //mai usare <any>
        .subscribe(result => {
          this.addressToCustomerAddress = {
            addressID: result.addressId,  //addressId anziche addressID perche result è tipo any quindi per accedervi il nome deve essere esattamento lo stesso del result da BE
            customerID: this.loggedUser!.oldCustomerId2 ? this.loggedUser!.oldCustomerId2 : this.loggedUser!.userId,  //operatore ternario che controlla se è utente nuovo o vecchio
            addressType: this.formAddress!.addressType
            
          };
          console.log(this.addressToCustomerAddress + 'ciao')      

          this.http.post('https://localhost:7081/api/CustomerAddresses', this.addressToCustomerAddress)  // meglio un post solo e gestisco due post da backend
            .subscribe( result => console.log(result))
        })
    }
  
    updateAddress(updateAF: NgForm) {   //questo metodo va trasposto nel BE in un unico controller per evitare di fare chiamate multiple
      this.formAddress = updateAF.value;

      this.addressToAddress = {
        addressId: 0,
        addressLine1: this.formAddress!.addressLine1,
        addressLine2: this.formAddress!.addressLine2,
        city: this.formAddress!.city,  
        countryRegion: this.formAddress!.countryRegion,
        stateProvince: this.formAddress!.stateProvince,
        postalCode: this.formAddress!.postalCode
      }
      
      this.http.get<GetAddress>(`https://localhost:7081/api/Addresses/${this.address!.address.addressId}`) 
        .subscribe(result => {
          this.updateAddressForm = {
            addressId: result.addressId,
            addressLine1: this.addressToAddress!.addressLine1,
            addressLine2: this.addressToAddress!.addressLine2,
            city: this.addressToAddress!.city,  
            countryRegion: this.addressToAddress!.countryRegion,
            stateProvince: this.addressToAddress!.stateProvince,
            postalCode: this.addressToAddress!.postalCode,
            rowguid: result.rowguid,
            modifiedDate: new Date()
          };
          this.updateCustomerAddressForm = {
            customerId: this.loggedUser!.oldCustomerId2 ? this.loggedUser!.oldCustomerId2 : this.loggedUser!.userId,
            addressId: result.addressId,
            addressType: this.formAddress!.addressType,
            rowguid: result.rowguid,
            modifiedDate: new Date()
          }
          this.http.put(`https://localhost:7081/api/Addresses/${this.address!.address.addressId}`, this.updateAddressForm)
            .subscribe(result => console.log(result))
          this.http.put(`https://localhost:7081/api/CustomerAddresses/${this.address!.address.addressId}`, this.updateCustomerAddressForm)
            .subscribe(result => console.log(result))
          })
    }

  //#endregion


  //////////////////////////////////////////////////////////////// ORDER HISTORY /////////////////////////////////////////////////////////////////////////


  getOrderHistoryByCustomerId() {
    console.log(this.loggedUser)
    this.http.get<OrderHistory[]>(`https://localhost:7081/api/ViewCompleteOrderHistory/history/${this.loggedUser!.oldCustomerId2 ? this.loggedUser!.oldCustomerId2 : this.loggedUser!.userId}`)
      .subscribe(result => {
        this.orderHistory = result;
      } )
  }

  getLastPlacedOrder() {
    this.http.get<OrderHistory>(`https://localhost:7081/api/ViewCompleteOrderHistory/lastPlaced${this.loggedUser!.oldCustomerId2 ? this.loggedUser!.oldCustomerId2 : this.loggedUser!.userId}`)
      .subscribe(result => {
        this.lastPlacedOrder = result;
      })
  }
}
