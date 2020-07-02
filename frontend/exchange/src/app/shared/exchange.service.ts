import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ExchangeService {
    // URL to web api
    private exchangeUrl = '/api/exchange';

    constructor(private http: HttpClient) { }

    /** GET dolar exchange rate from the server */
    getDolarExchangeRate(): Observable<any> {
        const url = `${this.exchangeUrl}/${"dolar"}`;

        return this.http.get<any>(url);
    }

    /** GET dolar exchange rate from the server */
    getCanadianDolarExchangeRate(): Observable<any> {
        const url = `${this.exchangeUrl}/${"canadian"}`;

        return this.http.get<any>(url);
    }

    /** GET dolar exchange rate from the server */
    getRealExchangeRate(): Observable<any> {
        const url = `${this.exchangeUrl}/${"real"}`;

        return this.http.get<any>(url);
    }
}
