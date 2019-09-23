import { Component, OnInit, TemplateRef, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { EditFoodDto } from '../../../shared/service-proxies';

@Component({
  selector: 'app-edit-food',
  templateUrl: './edit-food.component.html',
  styleUrls: ['./edit-food.component.css']
})
export class EditFoodComponent implements OnInit {

  @ViewChild('modal', { static: false }) modal: ModalDirective;
  @Input() food =  new EditFoodDto();
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
