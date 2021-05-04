import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NotificationsService } from 'angular2-notifications';

@Component({
  selector: 'app-purchases',
  templateUrl: './purchases.component.html',
  styleUrls: ['./purchases.component.css']
})
export class PurchasesComponent implements OnInit {

  currencies = [];
  purchaseForm: FormGroup;
  isLoading = false;
  purchases = [];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private _notificationService: NotificationsService) { }

  ngOnInit(): void {
    this.purchaseForm = new FormGroup({
      userId: new FormControl('', Validators.required),
      currencyId: new FormControl('', Validators.required),
      quantity: new FormControl(0, [Validators.required, Validators.min(1)]),
    });

    this.http.get(this.baseUrl + 'api/CurrencyPrices/Currencies').subscribe((result: any) => {
      console.log(result)
      this.currencies = result;
    }, error => console.error(error));

  }

  createPurchase() {
    if(this.purchaseForm.invalid) {
      this.purchaseForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.http.post('api/Purchases', this.purchaseForm.value).subscribe(response => {
      this.getPurchases(this.purchaseForm.value.userId);
      this.isLoading = false;
      this.purchaseForm.reset();
      this._notificationService.success('Success', 'Puchase create successfully');
    }, response => {
      this.isLoading = false;
      if(response && response.error) {
        this._notificationService.error('Error', response.error.message)
      }
      console.log(response);
    });
  }

  getPurchases(userId: number) {
    this.http.get('api/Purchases?userId='+userId).subscribe((response:any) => {
      console.log(response)
      this.purchases = response;
    }, error => console.log(error))
  }
  
  get userId() { return this.purchaseForm.get('userId'); }
  get currencyId() { return this.purchaseForm.get('currencyId'); }
  get quantity() { return this.purchaseForm.get('quantity'); }

}
