import { FullPost } from 'src/app/models/FullPost';
import { PostComment } from './../../../models/Comment';
import { CommentsApiClient } from './../clients/CommentsApiClient';
import { Profile } from 'src/app/models/Profile';
import { NavigationService } from '../../common/services/NavigationService';
import { SignUpRequestDTO } from '../../../models/dto/SignUpRequestDTO';
import { SignInResponceDTO } from '../../../models/dto/SignInResponceDTO';
import { SignInRequestDTO } from '../../../models/dto/SignInRequestDTO';
import { CookieService } from '../../../services/CookieService';
import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { SignUpResponceDTO } from 'src/app/models/dto/SignUpResponceDTO';
import { AccountsApiClient } from '../../accounts/clients/AccountsApiClient';
import { PostBase } from 'src/app/models/PostBase';
import { FeedPost } from 'src/app/models/FeedPost';

@Injectable({
  providedIn: 'root',
})
export class CommentsService {
  constructor(
    private client: CommentsApiClient,
    private cookie: CookieService,
    private router: Router,
    private navigate: NavigationService
  ) {}
  async getCommentsByPostAsync(
    post: PostBase | FullPost,
    page: number
  ): Promise<PostComment[]> {
    try {
      let res = await this.client.getCommentsByPostAsync(post.id, page);
      return res;
    } catch (error) {
      console.log(error);
      return [];
    }
  }

  async createCommentToPostAsync(
    post: FullPost | FeedPost | PostBase | undefined,
    text: string
  ): Promise<PostComment | null> {
    try {
      if (post == undefined) return null;
      let res = await this.client.createCommentToPostAsync(post.id, text);
      return res;
    } catch (error) {
      console.log(error);
      return null;
    }
  }
}
