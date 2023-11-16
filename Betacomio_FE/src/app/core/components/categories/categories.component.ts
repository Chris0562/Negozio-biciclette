import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterStateSnapshot, UrlSegment } from '@angular/router';
import { ApiManagerService } from 'src/app/services/api-manager.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent{
  parentId: number = 0;

  constructor(public srv: ApiManagerService,
              private route: ActivatedRoute) { 
    //this.parentId = this.route.snapshot.params['id']  //questo lo prendo dall ngFor
    //this.srv.getCategories(this.parentId)  //parte al click della card
  }

}
