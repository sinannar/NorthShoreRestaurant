import { Component, OnInit, TemplateRef, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { BsModalService, BsModalRef, ModalDirective } from 'ngx-bootstrap/modal';
import { ShowMenuDto, ShowFoodDto, RestaurantServiceProxy, AddFoodToMenuDto, RemoveFoodFromMenuDto } from 'src/shared/service-proxies';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-edit-menu',
  templateUrl: './edit-menu.component.html',
  styleUrls: ['./edit-menu.component.css']
})
export class EditMenuComponent implements OnInit {
  @ViewChild('modal', { static: false }) modal: ModalDirective;
  @Output() saved = new EventEmitter();

  foodsInMenu: ShowFoodDto[];
  foodsNotInMenu: ShowFoodDto[];

  // tslint:disable-next-line: variable-name
  _menu: ShowMenuDto;
  @Input() set menu(value: ShowMenuDto) {
    this._menu = value;
    this.intializeEditMenu();
  }
  get menu(): ShowMenuDto {
    return this._menu;
  }

  constructor(
    private modalService: BsModalService,
    private restaurantService: RestaurantServiceProxy) { }

  setNewMapping(food: ShowFoodDto) {
    const request = new AddFoodToMenuDto();
    request.menuId = this.menu.id;
    request.foodIds = [food.id];
    this.restaurantService.addFoodToMenu(request).subscribe(() => {
      this.intializeEditMenu();
      this.saved.emit(null);
    });
  }
  deleteFoodMapping(food: ShowFoodDto) {
    const request = new RemoveFoodFromMenuDto();
    request.menuId = this.menu.id;
    request.foodId = food.id;
    this.restaurantService.removeFoodFromMenu(request).subscribe(() => {
      this.intializeEditMenu();
      this.saved.emit(null);
    });
  }

  intializeEditMenu() {
    if (this.menu && this.menu.id) {
      const request = forkJoin(
        this.restaurantService.listFoodsInMenu(this.menu.id),
        this.restaurantService.listFoodsNotInMenu(this.menu.id)
      );

      request.subscribe(result => {
        this.foodsInMenu = result[0];
        this.foodsNotInMenu = result[1];
      });
    }
  }

  show() {
    this.modal.show();
    this.intializeEditMenu();
  }

  hide() {
    this.modal.hide();
  }

  ngOnInit() {
  }

}
