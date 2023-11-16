import { Component } from '@angular/core';
import { ApiManagerService } from 'src/app/services/api-manager.service';

@Component({
  selector: 'app-parent-categories',
  templateUrl: './parent-categories.component.html',
  styleUrls: ['./parent-categories.component.css']
})
export class ParentCategoriesComponent {  

  

  constructor(public srv: ApiManagerService) { } 
  
  getCategories(id: number, parentName:string){
    this.srv.parentCategoryName = parentName
    this.srv.getCategories(id) 
  }

}
