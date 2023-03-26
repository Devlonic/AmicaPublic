import { AccountsService } from '../../accounts/services/AccountsService';
import { Inject, Injectable } from '@angular/core';
import { AMICA_API_BASE_URL } from 'src/app/clients/common/AMICA_API_BASE_URL';
import { throwException } from 'src/app/clients/common/throwException';
import { Profile } from 'src/app/models/Profile';

@Injectable({
  providedIn: 'root',
})
export class ProfilesApiClient {
  private http: {
    fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
  };
  private baseUrl: string;
  protected jsonParseReviver: ((key: string, value: any) => any) | undefined =
    undefined;

  constructor(
    private accounts: AccountsService,
    @Inject(AMICA_API_BASE_URL) baseUrl?: string
  ) {
    this.http = window as any;
    this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : '';
  }

  /**
   * Get profile by profile_nickname
   * @return Success
   */
  public async getProfileAsync(profile_nickname: string): Promise<Profile> {
    let url_ = this.baseUrl + '/api/Profiles/Nickname/';
    if (profile_nickname === null)
      throw new Error("The parameter 'profile_nickname' cannot be null.");
    url_ += profile_nickname;
    let headers = new Headers();

    let s = this.accounts.getToken();
    if (s != undefined) headers.append('authorization', s);

    let options_: RequestInit = {
      method: 'GET',
      headers: headers,
      credentials: 'include',
    };

    var res = await this.http.fetch(url_, options_);
    return await this.processProfiles(res);
  }
  /**
   * Get profile by id
   * @return Success
   */
  public async getProfileByIdAsync(id: number): Promise<Profile> {
    let url_ = this.baseUrl + '/api/profiles/';
    if (id === null) throw new Error("The parameter 'id' cannot be null.");
    url_ += id;

    let headers = new Headers();

    let s = this.accounts.getToken();
    if (s != undefined) headers.append('authorization', s);

    let options_: RequestInit = {
      method: 'GET',
      headers: headers,
      credentials: 'include',
    };

    var res = await this.http.fetch(url_, options_);
    return await this.processProfiles(res);
  }

  protected async processProfiles(response: Response): Promise<Profile> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    var _responseText = await response.text();
    switch (status) {
      case 200:
        return JSON.parse(_responseText) as Profile;
      case 401:
        return throwException(
          'Unauthenticated request (Wrong, Missing or Expired token)',
          status,
          _responseText,
          _headers
        );
      case 404:
        return throwException('Not found', status, _responseText, _headers);
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
   * Get profiles by part
   * @return Success
   */
  public async scearchProfilesByNickNameAsync(
    part: string
  ): Promise<Profile[]> {
    let url_ = this.baseUrl + '/api/profiles/find/';
    if (part === null) throw new Error("The parameter 'part' cannot be null.");
    url_ += part;

    let headers = new Headers();

    let s = this.accounts.getToken();
    if (s != undefined) headers.append('authorization', s);

    let options_: RequestInit = {
      method: 'GET',
      headers: headers,
      credentials: 'include',
    };

    var res = await this.http.fetch(url_, options_);
    return await this.processScearchProfilesByNickNameAsync(res);
  }

  protected async processScearchProfilesByNickNameAsync(
    response: Response
  ): Promise<Profile[]> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    var _responseText = await response.text();
    switch (status) {
      case 200:
        return JSON.parse(_responseText) as Profile[];
      case 401:
        return throwException(
          'Unauthenticated request (Wrong, Missing or Expired token)',
          status,
          _responseText,
          _headers
        );
      case 404:
        return throwException('Not found', status, _responseText, _headers);
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
