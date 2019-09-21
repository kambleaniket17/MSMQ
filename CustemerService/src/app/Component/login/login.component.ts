import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ServiceService } from 'src/app/Service/service.service';
import { Navigation } from 'selenium-webdriver';
import { Router } from '@angular/router';
import { empty } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  LoginForm=new FormGroup({
    UserName:new FormControl(),
    Password:new FormControl()
  })
  constructor(private service:ServiceService,private router:Router) { }

  ngOnInit() {
  }
Login(){
console.log(this.LoginForm.value);
if(this.LoginForm.value.UserName == null && this.LoginForm.value.Password == null){
  alert("Please Fill The Form")
}
this.service.Login(this.LoginForm.value).subscribe(response=>{
  if(response[0]==undefined){
    alert("Invalide UserName And Password")
    this.LoginForm.reset();
    return
  }
  console.log(response[0]);
  localStorage.setItem("token",response[1]);
  
 alert("Last Login Form="+response[0]);
 this.router.navigateByUrl("Merchant");
})
}
}