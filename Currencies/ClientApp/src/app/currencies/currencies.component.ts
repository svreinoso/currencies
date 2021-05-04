import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CurrencyPrice } from '../models/currencyPrice';

@Component({
  selector: 'app-currencies',
  templateUrl: './currencies.component.html',
  styleUrls: ['./currencies.component.css']
})
export class CurrenciesComponent implements OnInit {

  public currencies: CurrencyPrice[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router) {
      
  }

  ngOnInit(): void {
    this.getCurrencies();
  }

  getCurrencies() {
    this.http.get<CurrencyPrice[]>(this.baseUrl + 'api/CurrencyPrices').subscribe(result => {
      this.currencies = result;
    }, error => console.error(error));
  }

  refresh() {
    this.getCurrencies();
  }

  buy(currencyName: string) {
    this.router.navigate(["/purchases/" + currencyName]);
  }

}

