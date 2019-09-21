import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatRadioModule, MatFormFieldModule, MatCardModule,MatButtonModule, MatButton, MatInput, MatInputModule} from '@angular/material';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddDataComponent } from './Component/add-data/add-data.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { FlexLayoutModule } from "@angular/flex-layout";
import { LoginComponent } from './Component/login/login.component';
import { RegistrationComponent } from './Component/registration/registration.component';
import { MerchantComponent } from './Component/merchant/merchant.component';
//import { ServiceService } from './Service/service.service';

@NgModule({
  declarations: [
    AppComponent,
    AddDataComponent,
    LoginComponent,
    RegistrationComponent,
    MerchantComponent,
    
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MatRadioModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatCardModule,
    HttpClientModule,
    FlexLayoutModule,
    MatButtonModule,
    MatInputModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
