import { NavigationService } from './../common/services/NavigationService';
import { AppPrimaryRoutingModule } from './../../app-primary-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginWidgetComponent } from './widgets/login-widget/login-widget.component';
import { FormsModule } from '@angular/forms';
import { RegistrationWidgetComponent } from './widgets/registration-widget/registration-widget.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { RegistrationPageComponent } from './pages/registration-page/registration-page.component';
import { AccountsPageComponent } from './pages/accounts-page/accounts-page.component';
import { AMICA_API_BASE_URL } from 'src/app/clients/common/AMICA_API_BASE_URL';
import { CookieService } from 'src/app/services/CookieService';
import { AccountsApiClient } from './clients/AccountsApiClient';
import { AccountsService } from './services/AccountsService';
import { CommonComponentsModule } from '../common/common-components.module';
import { AMICA_CAPTCHA_V2_KEY } from 'src/app/clients/common/AMICA_CAPTCHA_V2_KEY';

import { environment } from '../../../environments/environment';
import {
  RECAPTCHA_SETTINGS,
  RecaptchaFormsModule,
  RecaptchaModule,
  RecaptchaSettings,
} from 'ng-recaptcha';

@NgModule({
  declarations: [
    LoginWidgetComponent,
    RegistrationWidgetComponent,
    LoginPageComponent,
    RegistrationPageComponent,
    AccountsPageComponent,
  ],
  providers: [
    AccountsApiClient,
    AccountsService,
    CookieService,
    NavigationService,
    { provide: AMICA_API_BASE_URL, useValue: 'http://localhost:4200' },
    {
      provide: AMICA_CAPTCHA_V2_KEY,
      useValue: '6LfA5zkkAAAAAM4ehVUdmB7-MKC6Gmacb6spGdlY',
    },
  ],
  imports: [
    CommonModule,
    FormsModule,
    AppPrimaryRoutingModule,
    CommonComponentsModule,
    RecaptchaModule,
  ],
})
export class AccountsModule {}
