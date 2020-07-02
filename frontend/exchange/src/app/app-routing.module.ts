import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ExchangeRateComponent } from './pages/exchange-rate/exchange-rate.component';
import { PurchaseComponent } from './pages/purchase/purchase.component';
import { HomeComponent } from './pages/home/home.component';


const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: HomeComponent },
    { path: 'exchange', component: ExchangeRateComponent },
    { path: 'purchase', component: PurchaseComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
