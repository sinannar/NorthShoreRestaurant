import { Component, OnInit, ViewChild } from '@angular/core';
import { CreateMenuComponent } from '../create-menu/create-menu.component';
import { EditMenuComponent } from '../edit-menu/edit-menu.component';
import { RestaurantServiceProxy, ShowMenuDto, CreateMenuDto } from 'src/shared/service-proxies';

@Component({
  selector: 'app-list-menu',
  templateUrl: './list-menu.component.html',
  styleUrls: ['./list-menu.component.css'],
  providers: [RestaurantServiceProxy]
})
export class ListMenuComponent implements OnInit {
  @ViewChild('createMenuModal', { static: false }) createMenuModal: CreateMenuComponent;
  @ViewChild('editMenuModal', { static: false }) editMenuModal: EditMenuComponent;

  menus: ShowMenuDto[];

  createMenuDto = new CreateMenuDto();
  editMenuDto = new ShowMenuDto();

  constructor(
    // tslint:disable-next-line: variable-name
    private _restaurantService: RestaurantServiceProxy,
  ) {
  }

  ngOnInit() {
    this.list();
  }

  list() {
    this._restaurantService.listMenus().subscribe(result => {
      this.menus = result;
    });
  }

  createMenu() {
    this.createMenuDto = new CreateMenuDto();
    this.createMenuModal.show();
  }

  created() {
    this._restaurantService.createMenu(this.createMenuDto).subscribe(() => {
      this.list();
    });
  }

  editMenu(menu: ShowMenuDto) {
    this.editMenuDto = menu;
    this.editMenuModal.show();
  }

  updated() {
    this.list();
  }

  deleteMenu(menu: ShowMenuDto) {
    this._restaurantService.deleteMenu(menu.id).subscribe(() => {
      this.list();
    });
  }

  refresh($event: any) {
    this.list();
  }

}
