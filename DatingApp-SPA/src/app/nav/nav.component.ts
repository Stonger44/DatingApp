import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      console.log('Login successful.');
    }, error => {
      console.log('Login failed.');
    });
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token; // !! returns true or false if the constant variable is empty or not
  }

  logout() {
    localStorage.removeItem('token');
    console.log('Logout successful.');
  }

}
