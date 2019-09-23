import { Component, OnInit } from '@angular/core';
import { ValuesServiceProxy } from 'src/shared/service-proxies';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ValuesServiceProxy]
})
export class AppComponent implements OnInit {
  values: string[];
  constructor(public service: ValuesServiceProxy) {

  }

  ngOnInit() {
    this.service.getValues().subscribe(result => {
      this.values = result;
    });
  }
}
