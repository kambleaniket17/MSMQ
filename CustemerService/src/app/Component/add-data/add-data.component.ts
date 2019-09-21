import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import {MatRadioModule,MatRadioButton,MatRadioGroup} from '@angular/material/';
import { from, Observable } from 'rxjs';
import { ServiceService } from 'src/app/Service/service.service';

@Component({
  selector: 'app-add-data',
  templateUrl: './add-data.component.html',
  styleUrls: ['./add-data.component.css']
})
export class AddDataComponent implements OnInit {
  toggle:boolean=false;
  favoriteSeason: string;
  Name:string;
  seasons: string[] = ['Customer', 'Merchant'];
  CustomerForm=new FormGroup({
   Name:new FormControl(),
   Mobile:new FormControl(),
   Email:new FormControl(),
   City:new FormControl(),
   Product:new FormControl()
  })
  // MerchantForm=new FormGroup({
  //   Name:new FormControl(),
  //   Mobile:new FormControl(),
  //   Email:new FormControl(),
   
  // })
  constructor(private service:ServiceService) { }

  ngOnInit() {
    this.favoriteSeason = this.seasons[1];
  }
  changeComboo(event) {
    console.log('chnaged', event && event.value);
  }
  AddCustomer(){
    
     this.service.AddCustomer(this.CustomerForm.value).subscribe(Response=>{
       if(Response==true){
         alert("Added Successfully")
         this.CustomerForm.reset();
       }
     })
    
    
  }
  AddMerchant(){
    
    this.service.AddMerchant(this.CustomerForm.value).subscribe(data=>{
      console.log(data);
      this.CustomerForm.reset();
    })
   
   
 }

  customerInfo(){
    this.toggle=false;

  }
  merchantInfo(){
this.toggle=true;
  }

}
