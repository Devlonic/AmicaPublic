import { SignUpResponceDTO } from '../../../models/dto/SignUpResponceDTO';
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { AMICA_API_BASE_URL } from 'src/app/clients/common/AMICA_API_BASE_URL';
import { FileParameter } from 'src/app/clients/common/FileParameter';
import { throwException } from 'src/app/clients/common/throwException';
import { SignInResponceDTO } from 'src/app/models/dto/SignInResponceDTO';
import { SignUpRequestDTO } from 'src/app/models/dto/SignUpRequestDTO';
import { AccountsService } from '../../accounts/services/AccountsService';
import { Profile } from 'src/app/models/Profile';

@Injectable({
  providedIn: 'root',
})
export class FollowersApiClient {
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
   * Follow by profile_id and identity token
   * @return Success
   */
  async followAsync(profile: Profile): Promise<void> {
    let url_ = this.baseUrl + '/api/Followers/{profile_id}/Follow';
    if (profile.id === undefined || profile.id === null)
      throw new Error("The parameter 'profile_id' must be defined.");
    url_ = url_.replace('{profile_id}', encodeURIComponent('' + profile.id));
    url_ = url_.replace(/[?&]$/, '');

    let headers = new Headers();

    let s = this.accounts.getToken();
    if (s != undefined) headers.append('authorization', s);

    let options_: RequestInit = {
      method: 'POST',
      headers: headers,
      credentials: 'include',
    };

    var res = await this.http.fetch(url_, options_);
    return await this.processFollowAsync(res, profile);
  }

  protected async processFollowAsync(
    response: Response,
    profile: Profile
  ): Promise<void> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    var _responseText = await response.text();
    switch (status) {
      case 200:
        profile.isRequesterFollowsProfile = true;
        return;
      case 401:
        return throwException(
          'Unauthenticated request (Wrong, Missing or Expired token)',
          status,
          _responseText,
          _headers
        );
      case 403:
        return throwException(
          'Any user problems with request (Already followed, profile not found, etc...)',
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
   * UnFollow by profile_id and identity token
   * @return Success
   */
  async unFollowAsync(profile: Profile): Promise<void> {
    let url_ = this.baseUrl + '/api/Followers/{profile_id}/UnFollow';
    if (profile.id === undefined || profile.id === null)
      throw new Error("The parameter 'profile_id' must be defined.");
    url_ = url_.replace('{profile_id}', encodeURIComponent('' + profile.id));
    url_ = url_.replace(/[?&]$/, '');

    let headers = new Headers();

    let s = this.accounts.getToken();
    if (s != undefined) headers.append('authorization', s);

    let options_: RequestInit = {
      method: 'POST',
      headers: headers,
      credentials: 'include',
    };

    var res = await this.http.fetch(url_, options_);
    return await this.processUnFollowAsync(res, profile);
  }

  protected async processUnFollowAsync(
    response: Response,
    profile: Profile
  ): Promise<void> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    var _responseText = await response.text();
    switch (status) {
      case 200:
        profile.isRequesterFollowsProfile = false;
        return;
      case 401:
        return throwException(
          'Unauthenticated request (Wrong, Missing or Expired token)',
          status,
          _responseText,
          _headers
        );
      case 403:
        return throwException(
          'Any user problems with request (Already followed, profile not found, etc...)',
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
   * Get followers by profile id with pagination
   * @param page (optional)
   * @return List of followers profile_id
   */
  public async getFollowersAsync(
    profile_id: number,
    page: number | undefined
  ): Promise<Profile[]> {
    let url_ = this.baseUrl + '/api/Followers/{profile_id}/Followers?';
    if (profile_id === undefined || profile_id === null)
      throw new Error("The parameter 'profile_id' must be defined.");
    url_ = url_.replace('{profile_id}', encodeURIComponent('' + profile_id));
    if (page === null) throw new Error("The parameter 'page' cannot be null.");
    else if (page !== undefined)
      url_ += 'Page=' + encodeURIComponent('' + page) + '&';
    url_ = url_.replace(/[?&]$/, '');

    let headers = new Headers();

    let s = this.accounts.getToken();
    if (s != undefined) headers.append('authorization', s);

    let options_: RequestInit = {
      method: 'GET',
      headers: headers,
      credentials: 'include',
    };

    var res = await this.http.fetch(url_, options_);
    return await this.processFollowersAsync(res);
  }

  /**
   * Get followings by profile id with pagination
   * @param page (optional)
   * @return List of followings profile_id
   */
  public async getFollowingsAsync(
    profile_id: number,
    page: number | undefined
  ): Promise<Profile[]> {
    let url_ = this.baseUrl + '/api/Followers/{profile_id}/Followings?';
    if (profile_id === undefined || profile_id === null)
      throw new Error("The parameter 'profile_id' must be defined.");
    url_ = url_.replace('{profile_id}', encodeURIComponent('' + profile_id));
    if (page === null) throw new Error("The parameter 'page' cannot be null.");
    else if (page !== undefined)
      url_ += 'Page=' + encodeURIComponent('' + page) + '&';
    url_ = url_.replace(/[?&]$/, '');

    let headers = new Headers();

    let s = this.accounts.getToken();
    if (s != undefined) headers.append('authorization', s);

    let options_: RequestInit = {
      method: 'GET',
      headers: headers,
      credentials: 'include',
    };

    var res = await this.http.fetch(url_, options_);
    return await this.processFollowersAsync(res);
  }

  protected async processFollowersAsync(
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
      case 403:
        return throwException('Forbiden', status, _responseText, _headers);
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
