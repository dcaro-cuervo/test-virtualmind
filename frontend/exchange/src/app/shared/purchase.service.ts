import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PurchaseService {
    // URL to web api
    private purchaseUrl = '/api/purchases/';

    private httpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    
    constructor(private http: HttpClient) { }

    /** POST purchase an amount of currency from a user from the server */
    postPurchase(purchase): Observable<any> {
        return this.http.post<any>(this.purchaseUrl, purchase, this.httpOptions)
    }
}
