import { Inject } from '@angular/core';
import { AMICA_API_BASE_URL } from 'src/app/clients/common/AMICA_API_BASE_URL';
import { throwException } from 'src/app/clients/common/throwException';
import { AccountsService } from '../../accounts/services/AccountsService';
import { Injectable } from '@angular/core';
import { Profile } from 'src/app/models/Profile';

@Injectable({
  providedIn: 'root',
})
export class LikesApiClient {
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
   * Like post by post_id and identity token
   * @return true if success overvise false
   */
  public async likeAsync(post_id: number): Promise<boolean> {
    let url_ = this.baseUrl + '/api/Likes/Post/{post_id}/Like';
    if (post_id === undefined || post_id === null)
      throw new Error("The parameter 'post_id' must be defined.");
    url_ = url_.replace('{post_id}', encodeURIComponent('' + post_id));
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
    return await this.processLikeAsync(res);
  }

  protected async processLikeAsync(response: Response): Promise<boolean> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    var _responseText = await response.text();
    switch (status) {
      case 200:
      case 201:
      case 204:
        return true;
      case 401:
        return throwException(
          'Unauthenticated request (Wrong, Missing or Expired token)',
          status,
          _responseText,
          _headers
        );
      case 403:
        return throwException(
          'Any user problems with request (already acted, post not found, etc...)',
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
   * unLike post by post_id and identity token
   * @return true if success overvise false
   */
  public async unLikeAsync(post_id: number): Promise<boolean> {
    let url_ = this.baseUrl + '/api/Likes/Post/{post_id}/UnLike';
    if (post_id === undefined || post_id === null)
      throw new Error("The parameter 'post_id' must be defined.");
    url_ = url_.replace('{post_id}', encodeURIComponent('' + post_id));
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
    return await this.processLikeAsync(res);
  }

  /**
   * Get likers of post by post_id
   * @param page (optional)
   * @return Success
   */
  public async getLikersAsync(
    post_id: number,
    page: number | undefined
  ): Promise<Profile[]> {
    let url_ = this.baseUrl + '/api/Likes/Post/{post_id}/Likers?';
    if (post_id === undefined || post_id === null)
      throw new Error("The parameter 'post_id' must be defined.");
    url_ = url_.replace('{post_id}', encodeURIComponent('' + post_id));
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
    return await this.processLikersAsync(res);
  }

  protected async processLikersAsync(response: Response): Promise<Profile[]> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    var _responseText = await response.text();
    switch (status) {
      case 200:
      case 201:
      case 204:
        return JSON.parse(_responseText) as Profile[];
      case 401:
        return throwException(
          'Unauthenticated request (Wrong, Missing or Expired token)',
          status,
          _responseText,
          _headers
        );
      case 404:
        return throwException(
          'Post not found',
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
