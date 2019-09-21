import { Injectable } from '@angular/core';
import { Component } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { rootRoute } from '@angular/router/src/router_module';
import { environment } from './../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class ServiceService {

  constructor(private httpService:HttpClient) { }

  AddCustomer(data){
    console.log(data);
   return this.httpService.post(environment.Url + "User/AddCustomer",data);
  }
  AddMerchant(data){
    console.log(data,environment.Url);
   return this.httpService.post(environment.Url + "User/AddMerchant",data);
  }
  Login(data){
   return this.httpService.post(environment.Url + "Admin/login",data);
  }
  Register(data){
    console.log(data);
    
    return this.httpService.post(environment.Url + "Admin/Register",data);
  }
  UpdateProduct(data){
    console.log(data);
    
    return this.httpService.post(environment.Url + "",data);
  }
} 