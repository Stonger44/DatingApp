import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  activateRegisterMode() {
    this.registerMode = true;
  }

  deactivateRegisterMode(registerMode: boolean) {
    this.registerMode = registerMode; // registerMode should be emitted as false from register component
  }

}
