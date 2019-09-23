import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { AccountServiceProxy, LoginDto } from 'src/shared/service-proxies';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [AccountServiceProxy]
})
export class LoginComponent implements OnInit {
  model = new LoginDto();
  constructor(private authService: AuthService, private accountServiceProxy: AccountServiceProxy) { }

  ngOnInit() {
  }

  login() {
    this.accountServiceProxy.logIn(this.model).subscribe(result => {
      this.authService.isAuthenticated = true;
      this.authService.token = result;
    });
  }
}
