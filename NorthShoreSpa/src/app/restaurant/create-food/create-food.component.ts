import { Component, OnInit, TemplateRef, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { CreateFoodDto } from '../../../shared/service-proxies';

@Component({
  selector: 'app-create-food',
  templateUrl: './create-food.component.html',
  styleUrls: ['./create-food.component.css']
})
export class CreateFoodComponent implements OnInit {
  @ViewChild('modal', { static: false }) modal: ModalDirective;
  @Input() food: CreateFoodDto;
  @Output() saved = new EventEmitter();

  constructor(private modalService: BsModalService) {}

  show() {
    this.modal.show();
  }

  hide() {
    this.modal.hide();
  }

  save() {
    this.saved.emit(null);
    this.hide();
  }

  ngOnInit(): void {
  }

}
