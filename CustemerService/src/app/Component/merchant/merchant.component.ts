import { Component, OnInit } from '@angular/core';
import * as XLSX from 'ts-xlsx';
import { FormControl, FormGroup } from '@angular/forms';
import { ServiceService } from 'src/app/Service/service.service';
@Component({
  selector: 'app-merchant',
  templateUrl: './merchant.component.html',
  styleUrls: ['./merchant.component.css']
})
export class MerchantComponent implements OnInit {
  arrayBuffer:any;
  file:File;
  ExcelData;
  Name;
  item:{
    Name:string
  }
  
  toggle:boolean=false;
  Product;
  Price;
  Quantity;
  TotalAmount;
  // ProductInfo=new FormGroup({
  //   Name:new FormControl(),
  //   Product:new FormControl(),
  //   Price:new FormControl(),
  //   Quantity:new FormControl(),
  //   TotalAmount:new FormControl()
  // })
  constructor(private service:ServiceService) { }

  ngOnInit() {
  }
  incomingfile(event) 
  {
  this.file= event.target.files[0]; 
  }

 Upload() {
  this.toggle=true;
      let fileReader = new FileReader();
        fileReader.onload = (e) => {
            this.arrayBuffer = fileReader.result;
            var data = new Uint8Array(this.arrayBuffer);
            var arr = new Array();
            for(var i = 0; i != data.length; ++i) arr[i] = String.fromCharCode(data[i]);
            var bstr = arr.join("");
            var workbook = XLSX.read(bstr, {type:"binary"});
            var first_sheet_name = workbook.SheetNames[0];
            var worksheet = workbook.Sheets[first_sheet_name];
            this.ExcelData= XLSX.utils.sheet_to_json(worksheet,{raw:true});
           console.log(this.ExcelData);
           
        }
        fileReader.readAsArrayBuffer(this.file);
}
UpdateProduct(){
  console.log(this.ExcelData);

  for (let i = 0; i < this.ExcelData.length; i++) {
    if(this.ExcelData[i].Quantity>1){
      this.ExcelData.TotalAmount
      this.ExcelData[i].TotalAmount = this.ExcelData[i].Quantity * this.ExcelData[i].Price;
      console.log(this.ExcelData[i].TotalAmount);
    }
    else{
      this.ExcelData[i].TotalAmount = this.ExcelData[i].Price;
     
    }
   this.service.UpdateProduct(this.ExcelData).subscribe(response=>{
     console.log(response);
     
   })
  }
  
}
}