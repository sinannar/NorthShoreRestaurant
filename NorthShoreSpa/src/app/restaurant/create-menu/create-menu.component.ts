import { Component, OnInit, TemplateRef, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { BsModalService, BsModalRef, ModalDirective } from 'ngx-bootstrap/modal';
import { CreateMenuDto } from 'src/shared/service-proxies';

@Component({
  selector: 'app-create-menu',
  templateUrl: './create-menu.component.html',
  styleUrls: ['./create-menu.component.css']
})
export class CreateMenuComponent implements OnInit {
  @ViewChild('modal', { static: false }) modal: ModalDirective;
  @Input() menu: CreateMenuDto;
  @Output() saved = new EventEmitter();

  constructor(private modalService: BsModalService) { }

  show() {
    this.modal.show();
  }

  save() {
    this.saved.emit(null);
    this.hide();
  }

  hide() {
    this.modal.hide();
  }

  ngOnInit() {
  }

}
