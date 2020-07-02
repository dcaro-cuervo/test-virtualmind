import { Component, OnInit } from '@angular/core';

import { ExchangeService } from '../../shared/exchange.service';
import { forkJoin } from 'rxjs';

@Component({
    selector: 'app-exchange-rate',
    templateUrl: './exchange-rate.component.html',
    styleUrls: ['./exchange-rate.component.scss']
})
export class ExchangeRateComponent implements OnInit {

    dolarExchange;
    realExchange;
    loading;

    constructor(private exchangeService: ExchangeService) { }

    ngOnInit(): void {
        this.updateExchanges();
    }

    updateExchanges() {
        this.loading = true;

        forkJoin([this.exchangeService.getDolarExchangeRate(), this.exchangeService.getRealExchangeRate()]).subscribe(exchanges => {
            this.dolarExchange = exchanges[0];
            this.realExchange = exchanges[1];

            this.loading = false;
        });
    }
}
