import { Component, OnInit } from '@angular/core';
import { ApiManagerService } from 'src/app/services/api-manager.service';
import { FormControl} from '@angular/forms';
import { Observable, Subject} from 'rxjs';
import { startWith, map, tap, debounceTime, first } from 'rxjs/operators';
import { ProductModels } from 'src/app/models/productModels';

@Component({
  selector: 'app-product-manager',
  templateUrl: './product-manager.component.html',
  styleUrls: ['./product-manager.component.css']
})
export class ProductManagerComponent implements OnInit{
  name: string | null = null;
  productNumber: string | null = null; 
  standardCost: number | null = null
  color: string | null = null;
  listPrice: number | null = null
  size: string | null = null;
  weight: number| null = null;
  productCategoryId: number | null = null
  productModelId: number | null = null;
  sellStartDate: Date | null = null;

  

  myControl = new FormControl('');
  
  filteredOptions: Observable<ProductModels[]> | undefined = undefined

  constructor(public srv: ApiManagerService) { }

  ngOnInit() {
    this.filteredOptions = this.myControl.valueChanges.pipe(  //nota i cambiamenti
      startWith(''),
      map(value => this._filter(value || '')))  //fa partire il filtro
  }

  private _filter(value: string): ProductModels[] {
    const filterValue = value.toLowerCase();
    return this.srv.productModels.filter(productModel => this.containsWord(productModel.nomeModello, value));   //fa partire containsWord
  }
  
  containsWord(text: string, value: string) {   //filtra
    text = text.toLowerCase();
    value = value.toString().toLowerCase();

    const searchWords = value.split(' ');

    return searchWords.every(word => text.includes(word));  //alla fine di tutto ritorna l'array di modelli filtrati
  }

  createTable(event: any) {
    if (this.myControl.value != '') {
      this.srv.getProductsAdmin(event)
    }    
  }
  
  getProductModels(id: string) { //metodo utilizzato nella select del form 'add new product'
      this.srv.getProductModels(id);
    }
}













































