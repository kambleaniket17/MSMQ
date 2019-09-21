import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddDataComponent } from './Component/add-data/add-data.component';
import { LoginComponent } from './Component/login/login.component';
import { RegistrationComponent } from './Component/registration/registration.component';
import { MerchantComponent } from './Component/merchant/merchant.component';

const routes: Routes = [
  {
    path:'',redirectTo:'Add',pathMatch:'full',
  },
  {
    path:'Add',component:AddDataComponent
  },
  {
    path:'login',component:LoginComponent
  },
  {
    path:'register',component:RegistrationComponent
  },
  {
    path:'Merchant',component:MerchantComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
