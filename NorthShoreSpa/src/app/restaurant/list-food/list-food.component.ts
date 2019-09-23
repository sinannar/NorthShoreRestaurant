import { Component, OnInit, ViewChild } from '@angular/core';
import { CreateFoodComponent } from '../create-food/create-food.component';
import { EditFoodComponent } from '../edit-food/edit-food.component';
import { ShowFoodDto, RestaurantServiceProxy, CreateFoodDto, EditFoodDto } from 'src/shared/service-proxies';

@Component({
  selector: 'app-list-food',
  templateUrl: './list-food.component.html',
  styleUrls: ['./list-food.component.css'],
  providers: [RestaurantServiceProxy]
})
export class ListFoodComponent implements OnInit {

  @ViewChild('createFoodModal', { static: false }) createFoodModal: CreateFoodComponent;
  @ViewChild('editFoodModal', { static: false }) editFoodModal: EditFoodComponent;
  foods: ShowFoodDto[];

  createFoodDto = new CreateFoodDto();
  editFoodDto = new EditFoodDto();
  constructor(
    // tslint:disable-next-line: variable-name
    private _restaurantService: RestaurantServiceProxy
  ) {
  }

  ngOnInit() {
    this.list();
  }

  list() {
    this._restaurantService.listFoods().subscribe(result => {
      this.foods = result;
    });
  }

  createFood() {
    this.createFoodDto = new CreateFoodDto();
    this.createFoodModal.show();
  }

  created() {
    this._restaurantService.createFood(this.createFoodDto).subscribe(() => {
      this.list();
    });
  }

  editFood(food: ShowFoodDto) {
    this.editFoodDto = food;
    this.editFoodModal.show();
  }

  edited() {
    this._restaurantService.editFood(this.editFoodDto).subscribe(() => {
      this.list();
    });
  }

  deleteFood(food: ShowFoodDto) {
    this._restaurantService.deleteFood(food.id).subscribe(() => {
      this.list();
    });
  }

  refresh($event: any) {
    this.list();
  }
}
