import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ServiceService } from 'src/app/Service/service.service';
import { Router } from '@angular/router';
import {MatInput} from '@angular/material/';
@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  RegistrationForm=new FormGroup({
    Name:new FormControl(),
       Mobile:new FormControl(),
      Email:new FormControl(),
      Password:new FormControl()
  })
  constructor(private service:ServiceService,private router:Router) { }

  ngOnInit() {
  }
  Register(){
    console.log(this.RegistrationForm.value);
    this.service.Register(this.RegistrationForm.value).subscribe(Response=>{
      console.log(Response);
      this.router.navigateByUrl("login");
    })
  }
}
