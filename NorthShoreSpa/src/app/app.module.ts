import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { environment } from '../environments/environment';
import { API_BASE_URL as api_url } from '../shared/service-proxies';

export function getRemoteServiceBaseUrl(): string {
  return environment.backEndUrl;
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    { provide: api_url, useFactory: getRemoteServiceBaseUrl },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
