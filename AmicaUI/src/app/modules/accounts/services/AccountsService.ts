import { Profile } from './../../../models/Profile';
import { NavigationService } from './../../common/services/NavigationService';
import { SignUpRequestDTO } from './../../../models/dto/SignUpRequestDTO';
import { SignInResponceDTO } from '../../../models/dto/SignInResponceDTO';
import { SignInRequestDTO } from '../../../models/dto/SignInRequestDTO';
import { CookieService } from '../../../services/CookieService';
import { AccountsApiClient } from './../clients/AccountsApiClient';
import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { SignUpResponceDTO } from 'src/app/models/dto/SignUpResponceDTO';

@Injectable({
  providedIn: 'root',
})
export class AccountsService {
  private client: AccountsApiClient;
  private cookie: CookieService;
  constructor(
    @Inject(AccountsApiClient) client: AccountsApiClient,
    @Inject(CookieService) cookie: CookieService,
    private router: Router,
    private navigate: NavigationService
  ) {
    this.client = client;
    this.cookie = cookie;
  }
  public getToken(): string | undefined {
    const token = this.cookie.getCurrentAuth()?.token;
    return token;
  }
  public getCurrentProfile(): Profile | undefined {
    const profile = this.cookie.getCurrentAuth()?.profile;
    return profile;
  }
  public isSignedIn(): boolean {
    const token = this.cookie.getCurrentAuth()?.token;
    return token != undefined && token != null;
  }
  public signOut() {
    this.cookie.deleteCurrentAuth();
    this.navigate.toLogin();
  }
  public async signInAsync(req: SignInRequestDTO): Promise<SignInResponceDTO> {
    var res = await this.client.signInAsync(req.login, req.password);
    if (res.isSignedIn == true) {
      this.cookie.setCurrentAuth(res);
      this.navigate.toHome();
    }
    return res;
  }
  public async signUpAsync(
    req: SignUpRequestDTO,
    needToSignIn: boolean = false
  ): Promise<SignUpResponceDTO> {
    var res = await this.client.signUpAsync(req);
    if (res.isSignedUp == true) {
      if (needToSignIn) {
        var authRes = await this.signInAsync({
          login: req.nickname,
          password: req.password,
        });
      }
    }
    return res;
  }
}
