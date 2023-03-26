import { FullPost } from 'src/app/models/FullPost';
import { ProfilePost } from './../../../models/ProfilePost';
import { AccountsService } from './../../accounts/services/AccountsService';
import { FeedPost } from './../../../models/FeedPost';
import { Inject, Injectable } from '@angular/core';
import { AMICA_API_BASE_URL } from 'src/app/clients/common/AMICA_API_BASE_URL';
import { FileParameter } from 'src/app/clients/common/FileParameter';
import { throwException } from 'src/app/clients/common/throwException';
import { PostCreateResponceDTO } from 'src/app/models/dto/PostCreateResponceDTO';

@Injectable({
  providedIn: 'root',
})
export class PostsApiClient {
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
   * Get full post by post_id
   * @return Full post
   */
  async getByIdAsync(post_id: number): Promise<FullPost | null> {
    let url_ = this.baseUrl + '/api/Posts/{post_id}';
    if (post_id === undefined || post_id === null)
      throw new Error("The parameter 'post_id' must be defined.");
    url_ = url_.replace('{post_id}', encodeURIComponent('' + post_id));
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
    return await this.processPostsGetAsync(res);
  }

  protected async processPostsGetAsync(
    response: Response
  ): Promise<FullPost | null> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    var _responseText = await response.text();
    switch (status) {
      case 200:
        return JSON.parse(_responseText) as FullPost;
      case 401:
        return throwException(
          'Unauthenticated request (Wrong, Missing or Expired token)',
          status,
          _responseText,
          _headers
        );
      case 404:
        return null;
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
   * Delete post by post_id
   * @return Post deleted
   */
  postsDelete(post_id: number): Promise<void> {
    let url_ = this.baseUrl + '/api/Posts/{post_id}';
    if (post_id === undefined || post_id === null)
      throw new Error("The parameter 'post_id' must be defined.");
    url_ = url_.replace('{post_id}', encodeURIComponent('' + post_id));
    url_ = url_.replace(/[?&]$/, '');

    let options_: RequestInit = {
      method: 'DELETE',
      headers: {},
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processPostsDelete(_response);
    });
  }

  protected processPostsDelete(response: Response): Promise<void> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    if (status === 200) {
      return response.text().then((_responseText) => {
        return;
      });
    } else if (status === 401) {
      return response.text().then((_responseText) => {
        return throwException(
          'Unauthenticated request (Wrong, Missing or Expired token)',
          status,
          _responseText,
          _headers
        );
      });
    } else if (status === 403) {
      return response.text().then((_responseText) => {
        return throwException(
          'Unauthorized request (User is not owner of requested post)',
          status,
          _responseText,
          _headers
        );
      });
    } else if (status === 404) {
      return response.text().then((_responseText) => {
        return throwException(
          'Post with id post_id not found',
          status,
          _responseText,
          _headers
        );
      });
    } else if (status === 500) {
      return response.text().then((_responseText) => {
        return throwException(
          'Any server problem. Until release returns Exception text',
          status,
          _responseText,
          _headers
        );
      });
    } else if (status !== 200 && status !== 204) {
      return response.text().then((_responseText) => {
        return throwException(
          'An unexpected server error occurred.',
          status,
          _responseText,
          _headers
        );
      });
    }
    return Promise.resolve<void>(null as any);
  }

  /**
   * Edit post by post_id
   * @param title (optional)
   * @return Post edited
   */
  postsPatch(post_id: number, title: string | undefined): Promise<void> {
    let url_ = this.baseUrl + '/api/Posts/{post_id}';
    if (post_id === undefined || post_id === null)
      throw new Error("The parameter 'post_id' must be defined.");
    url_ = url_.replace('{post_id}', encodeURIComponent('' + post_id));
    url_ = url_.replace(/[?&]$/, '');

    const content_ = new FormData();
    if (title === null || title === undefined)
      throw new Error("The parameter 'title' cannot be null.");
    else content_.append('Title', title.toString());

    let options_: RequestInit = {
      body: content_,
      method: 'PATCH',
      headers: {},
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processPostsPatch(_response);
    });
  }

  protected processPostsPatch(response: Response): Promise<void> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    if (status === 200) {
      return response.text().then((_responseText) => {
        return;
      });
    } else if (status === 401) {
      return response.text().then((_responseText) => {
        return throwException(
          'Unauthenticated request (Wrong, Missing or Expired token)',
          status,
          _responseText,
          _headers
        );
      });
    } else if (status === 403) {
      return response.text().then((_responseText) => {
        return throwException(
          'Unauthorized request (User is not owner of requested post)',
          status,
          _responseText,
          _headers
        );
      });
    } else if (status === 404) {
      return response.text().then((_responseText) => {
        return throwException(
          'Post with id post_id not found',
          status,
          _responseText,
          _headers
        );
      });
    } else if (status === 500) {
      return response.text().then((_responseText) => {
        return throwException(
          'Any server problem. Until release returns Exception text',
          status,
          _responseText,
          _headers
        );
      });
    } else if (status !== 200 && status !== 204) {
      return response.text().then((_responseText) => {
        return throwException(
          'An unexpected server error occurred.',
          status,
          _responseText,
          _headers
        );
      });
    }
    return Promise.resolve<void>(null as any);
  }

  /**
   * Get reduced posts by profile_id with pagination
   * @param page (optional)
   * @return No problem
   */
  public async getByProfileAsync(
    profile_id: number,
    page: number | undefined
  ): Promise<ProfilePost[]> {
    let url_ = this.baseUrl + '/api/Posts/GetByProfile/{profile_id}?';
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
    return await this.processGetByProfileAsync(res);
  }

  protected async processGetByProfileAsync(
    response: Response
  ): Promise<ProfilePost[]> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    var _responseText = await response.text();
    switch (status) {
      case 200:
        return JSON.parse(_responseText) as ProfilePost[];
      case 401:
        return throwException(
          'Unauthenticated request (Wrong, Missing or Expired token)',
          status,
          _responseText,
          _headers
        );
      case 404:
        return [];
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
   * Get reduced posts by profile_id by following for feed with pagination
   * @param page (optional)
   * @return No problem
   */
  public async getFeedsAsync(page: number | undefined): Promise<FeedPost[]> {
    let url_ = this.baseUrl + '/api/Posts/Feeds?';
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
    return await this.processFeedsAsync(res);
  }

  protected async processFeedsAsync(response: Response): Promise<FeedPost[]> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    var _responseText = await response.text();
    switch (status) {
      case 200:
        return JSON.parse(_responseText) as FeedPost[];
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

  /**
   * Create post by identity token
   * @param title (optional)
   * @param images (optional)
   * @return Post created
   */
  public async createPostAsync(
    title: string | undefined,
    images: FileParameter[] | undefined
  ): Promise<PostCreateResponceDTO> {
    let url_ = this.baseUrl + '/api/Posts';
    url_ = url_.replace(/[?&]$/, '');

    const content_ = new FormData();
    if (title === null || title === undefined)
      throw new Error("The parameter 'title' cannot be null.");
    else content_.append('Title', title.toString());
    if (images === null || images === undefined)
      throw new Error("The parameter 'images' cannot be null.");
    else
      images.forEach((item_) =>
        content_.append(
          'Images',
          item_.data,
          item_.fileName ? item_.fileName : 'Images'
        )
      );

    let headers = new Headers();

    let s = this.accounts.getToken();
    if (s != undefined) headers.append('authorization', s);

    let options_: RequestInit = {
      body: content_,
      method: 'POST',
      headers: headers,
      credentials: 'include',
    };

    var res = await this.http.fetch(url_, options_);
    return await this.processCreatePostAsync(res);
  }

  protected async processCreatePostAsync(
    response: Response
  ): Promise<PostCreateResponceDTO> {
    const status = response.status;
    let _headers: any = {};
    if (response.headers && response.headers.forEach) {
      response.headers.forEach((v: any, k: any) => (_headers[k] = v));
    }
    var _responseText = await response.text();
    switch (status) {
      case 200:
      case 201:
      case 202:
        return JSON.parse(_responseText) as PostCreateResponceDTO;
      case 400:
        return throwException(
          'Bad request(angular error)',
          status,
          _responseText,
          _headers
        );
      case 401:
        return throwException(
          'Unauthenticated request (Wrong, Missing or Expired token)',
          status,
          _responseText,
          _headers
        );
      case 409:
        return throwException(
          'Model validation error',
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
