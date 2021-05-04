import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { CurrenciesComponent } from './currencies/currencies.component';
import { PurchasesComponent } from './purchases/purchases.component';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { AppLoadingIndicatorComponent } from './app-loading-indicator/app-loading-indicator.component';
import { SimpleNotificationsModule } from 'angular2-notifications';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    CurrenciesComponent,
    PurchasesComponent,
    AppLoadingIndicatorComponent,
  ],
  imports: [
    SimpleNotificationsModule.forRoot({
      timeOut: 5000,
      showProgressBar: true,
      pauseOnHover: true,
      clickToClose: false,
      clickIconToClose: true,
      position: ['top', 'center']
    }),
    BrowserModule,
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    LoadingBarHttpClientModule,
    BrowserAnimationsModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: CurrenciesComponent, pathMatch: 'full' },
      { path: 'prices', component: CurrenciesComponent },
      { path: 'purchases', component: PurchasesComponent },
      { path: 'purchases/:currencyName', component: PurchasesComponent },
    ], { relativeLinkResolution: 'legacy' })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
