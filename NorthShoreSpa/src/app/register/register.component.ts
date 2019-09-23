import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { AccountServiceProxy, RegisterDto } from 'src/shared/service-proxies';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  providers: [AccountServiceProxy]
})
export class RegisterComponent implements OnInit {
  model = new RegisterDto();
  constructor(private authService: AuthService, private accountServiceProxy: AccountServiceProxy) { }

  ngOnInit() {
  }

  save() {
    this.accountServiceProxy.registerUser(this.model).subscribe(result => {
      this.authService.isAuthenticated = true;
      this.authService.token = result;
    });
  }

}
