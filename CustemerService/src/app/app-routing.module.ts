import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddDataComponent } from './Component/add-data/add-data.component';

const routes: Routes = [
  {
    path:'',redirectTo:'Add',pathMatch:'full',
  },
  {
    path:'Add',component:AddDataComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
