import { ProfilesModule } from './modules/profiles/profiles.module';
import { AccountsModule } from './modules/accounts/accounts.module';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { AccountsService } from './services/AccountsService';
import { CookieService } from './services/CookieService';
import { AMICA_API_BASE_URL } from './clients/common/AMICA_API_BASE_URL';
import { AccountsApiClient } from './clients/AccountsApiClient';

@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, HttpClientModule, AccountsModule, ProfilesModule],
  providers: [
    AccountsApiClient,
    AccountsService,
    CookieService,
    { provide: AMICA_API_BASE_URL, useValue: 'http://localhost:4200' },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
