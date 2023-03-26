import { SignUpResponceDTO } from './../../../models/dto/SignUpResponceDTO';
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { AMICA_API_BASE_URL } from 'src/app/clients/common/AMICA_API_BASE_URL';
import { FileParameter } from 'src/app/clients/common/FileParameter';
import { throwException } from 'src/app/clients/common/throwException';
import { SignInResponceDTO } from 'src/app/models/dto/SignInResponceDTO';
import { SignUpRequestDTO } from 'src/app/models/dto/SignUpRequestDTO';

@Injectable({
  providedIn: 'root',
})
export class AccountsApiClient {
  private http: {
    fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
  };
  private baseUrl: string;
  protected jsonParseReviver: ((key: string, value: any) => any) | undefined =
    undefined;

  constructor(@Inject(AMICA_API_BASE_URL) baseUrl?: string) {
    this.http = window as any;
    this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : '';
  }

  /**
   * Transform (login and password) to (identity token)
   * @param login (optional)
   * @param password (optional)
   * @return API responce with status and (if success) token
   */
  public async signInAsync(
    login: string | undefined,
    password: string | undefined
  ): Promise<SignInResponceDTO> {
    let url_ = this.baseUrl + '/api/Accounts/SignIn';
    url_ = url_.replace(/[?&]$/, '');

    const content_ = new FormData();
    // model validation
    if (login === null || login === undefined)
      throw new Error("The parameter 'login' cannot be null.");
    else content_.append('Login', login.toString());
    if (password === null || password === undefined)
      throw new Error("The parameter 'password' cannot be null.");
    else content_.append('Password', password.toString());

    let options_: RequestInit = {
      body: content_,
      method: 'POST',
      headers: {},
    };

    var res = await this.http.fetch(url_, options_);
    return await this.processSignInAsync(res);
  }

  protected async processSignInAsync(
    response: Response
  ): Promise<SignInResponceDTO> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    var _responseText = await response.text();

    switch (status) {
      case 200:
        return JSON.parse(_responseText) as SignInResponceDTO;
      case 422:
        return throwException(
          'Wrong login or password. Check credentials!',
          status,
          _responseText,
          _headers
        );
      case 500:
        return throwException('Server error', status, _responseText, _headers);
      default:
        return throwException(
          'Unexpected server error',
          status,
          _responseText,
          _headers
        );
    }
  }

  /**
   * Register new user (Create account and linked profile)
   * @param req (optional)
   * @return API responce with status
   */
  async signUpAsync(req: SignUpRequestDTO): Promise<SignUpResponceDTO> {
    let url_ = this.baseUrl + '/api/Accounts/SignUp';
    url_ = url_.replace(/[?&]$/, '');

    const content_ = new FormData();

    content_.append('Email', req.email.toString());
    content_.append('FullName', req.fullName.toString());
    content_.append('Nickname', req.nickname.toString());
    content_.append('Password', req.password.toString());
    content_.append(
      'ProfilePhoto',
      req.profilePhoto.data,
      req.profilePhoto.fileName ? req.profilePhoto.fileName : 'ProfilePhoto'
    );
    content_.append('CaptchaV2', req.captchaV2.toString());

    let options_: RequestInit = {
      body: content_,
      method: 'POST',
      headers: {},
    };

    var res = await this.http.fetch(url_, options_);
    return await this.processSignUpAsync(res);
  }

  protected async processSignUpAsync(
    response: Response
  ): Promise<SignUpResponceDTO> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    var _responseText = await response.text();

    switch (status) {
      case 200:
        return JSON.parse(_responseText) as SignUpResponceDTO;
      case 422:
        return throwException(
          'Input validation error',
          status,
          _responseText,
          _headers
        );
      case 500:
        return throwException('Server error', status, _responseText, _headers);
      default:
        return throwException(
          'Unexpected server error',
          status,
          _responseText,
          _headers
        );
    }
  }
}
