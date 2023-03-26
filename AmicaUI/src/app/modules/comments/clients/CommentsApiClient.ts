import { Inject, Injectable } from '@angular/core';
import { AMICA_API_BASE_URL } from 'src/app/clients/common/AMICA_API_BASE_URL';
import { throwException } from 'src/app/clients/common/throwException';
import { PostComment } from 'src/app/models/Comment';
import { AccountsService } from '../../accounts/services/AccountsService';

@Injectable({
  providedIn: 'root',
})
export class CommentsApiClient {
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
   * Get comments for post by post_id with pagination
   * @param page (optional)
   * @return Comments list
   */
  public async getCommentsByPostAsync(
    post_id: number,
    page: number | undefined
  ): Promise<PostComment[]> {
    let url_ = this.baseUrl + '/api/Comments/Post/{post_id}?';
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
    return await this.processGetCommentsByPostAsync(res);
  }

  protected async processGetCommentsByPostAsync(
    response: Response
  ): Promise<PostComment[]> {
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
        return JSON.parse(_responseText) as PostComment[];
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
   * Add comment to post by post_id and identity token
   * @param commentText (optional)
   * @return Status
   */
  public async createCommentToPostAsync(
    post_id: number,
    commentText: string | undefined
  ): Promise<PostComment> {
    let url_ = this.baseUrl + '/api/Comments/Post/{post_id}';
    if (post_id === undefined || post_id === null)
      throw new Error("The parameter 'post_id' must be defined.");
    url_ = url_.replace('{post_id}', encodeURIComponent('' + post_id));
    url_ = url_.replace(/[?&]$/, '');

    const content_ = new FormData();
    if (commentText === null || commentText === undefined)
      throw new Error("The parameter 'commentText' cannot be null.");
    else content_.append('CommentText', commentText.toString());

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
    return await this.processCreateCommentToPostAsync(res);
  }

  protected async processCreateCommentToPostAsync(
    response: Response
  ): Promise<PostComment> {
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
        return JSON.parse(_responseText) as PostComment;
      case 401:
        return throwException(
          'Unauthenticated request (Wrong, Missing or Expired token)',
          status,
          _responseText,
          _headers
        );
      case 403:
        return throwException(
          'Any user problems with request (post not found, etc...)',
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
   * Delete comment by comment_id and identity token
   * @return Success
   */
  postDelete(comment_id: string): Promise<void> {
    let url_ = this.baseUrl + '/api/Comments/Post/{comment_id}';
    if (comment_id === undefined || comment_id === null)
      throw new Error("The parameter 'comment_id' must be defined.");
    url_ = url_.replace('{comment_id}', encodeURIComponent('' + comment_id));
    url_ = url_.replace(/[?&]$/, '');

    let options_: RequestInit = {
      method: 'DELETE',
      headers: {},
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processPostDelete(_response);
    });
  }

  protected processPostDelete(response: Response): Promise<void> {
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
          'Any user problems with request (comment not found, user is not own this comment, etc...)',
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
   * Update comment by comment_id and identity token
   * @param newCommentText (optional)
   * @return Success
   */
  postPatch(
    comment_id: string,
    newCommentText: string | undefined
  ): Promise<void> {
    let url_ = this.baseUrl + '/api/Comments/Post/{comment_id}';
    if (comment_id === undefined || comment_id === null)
      throw new Error("The parameter 'comment_id' must be defined.");
    url_ = url_.replace('{comment_id}', encodeURIComponent('' + comment_id));
    url_ = url_.replace(/[?&]$/, '');

    const content_ = new FormData();
    if (newCommentText === null || newCommentText === undefined)
      throw new Error("The parameter 'newCommentText' cannot be null.");
    else content_.append('NewCommentText', newCommentText.toString());

    let options_: RequestInit = {
      body: content_,
      method: 'PATCH',
      headers: {},
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processPostPatch(_response);
    });
  }

  protected processPostPatch(response: Response): Promise<void> {
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
          'Any user problems with request (comment not found, user is not own this comment, etc...)',
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
}
