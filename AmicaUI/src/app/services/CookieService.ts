import { Injectable } from '@angular/core';
import { SignInResponceDTO } from '../models/dto/SignInResponceDTO';

@Injectable()
export class CookieService {
  private readonly AUTH_COOKIE_PATH = 'CURRENT_SESSION';

  private cookie: any;
  constructor() {
    this.cookie = document.cookie;
  }
  public getCookie(name: string): string {
    let val = window.localStorage.getItem(name);
    if (val == '' || val == null || val == undefined) return '';
    return val;
    // let ca: Array<string> = document.cookie.split(';');
    // let caLen: number = ca.length;
    // let cookieName = `${name}=`;
    // let c: string;

    // for (let i: number = 0; i < caLen; i += 1) {
    //   c = ca[i].replace(/^\s+/g, '');
    //   if (c.indexOf(cookieName) == 0) {
    //     return c.substring(cookieName.length, c.length);
    //   }
    // }
    // return '';
  }

  public deleteCookie(name: string) {
    console.log('cookie before ' + document.cookie);
    this.setCookie(name, '', -1);
    console.log('cookie after ' + document.cookie);
  }

  public setCookie(
    name: string,
    value: string,
    expireDays: number,
    path: string = ''
  ) {
    window.localStorage.setItem(name, value);

    // let d: Date = new Date();
    // d.setTime(d.getTime() + expireDays * 24 * 60 * 60 * 1000);
    // let expires: string = `expires=${d.toUTCString()}`;
    // let cpath: string = path ? `; path=${path}` : '';
    // document.cookie = `${name}=${value}; ${expires}${cpath}`;
  }

  public getCurrentAuth(): SignInResponceDTO | null {
    let val = this.getCookie(this.AUTH_COOKIE_PATH);
    if (val === undefined || val == '' || val == null) return null;
    return JSON.parse(val);
  }
  public setCurrentAuth(dto: SignInResponceDTO) {
    this.deleteCurrentAuth();
    this.setCookie(
      this.AUTH_COOKIE_PATH,
      JSON.stringify(dto),
      1,
      this.AUTH_COOKIE_PATH
    );
  }
  public deleteCurrentAuth() {
    this.deleteCookie(this.AUTH_COOKIE_PATH);
  }
}
