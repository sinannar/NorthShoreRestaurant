import { Component } from '@angular/core';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  login = false;
  register = false;
  constructor(public authService: AuthService) {
  }
  triggerLogin() {
    this.register = false;
    this.login = !this.login;
  }
  triggetRegister() {
    this.login = false;
    this.register = !this.register;
  }
  logout() {
    this.authService.isAuthenticated = false;
    this.authService.token = '';
  }
}
