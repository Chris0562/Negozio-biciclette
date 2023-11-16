import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiManagerService } from 'src/app/services/api-manager.service';

@Component({
  selector: 'app-product-models',
  templateUrl: './product-models.component.html',
  styleUrls: ['./product-models.component.css']
})
export class ProductModelsComponent {

  name: string = '';

  constructor(private route: ActivatedRoute,
              public srv: ApiManagerService) {
    
    this.name = this.route.snapshot.params['name']
    this.srv.getProductModels(this.name)
  }
}
