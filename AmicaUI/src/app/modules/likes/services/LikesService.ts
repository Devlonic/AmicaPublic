import { LikesApiClient } from './../clients/LikesApiClient';
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

@Injectable({
  providedIn: 'root',
})
export class LikesService {
  constructor(
    private client: LikesApiClient,
    private cookie: CookieService,
    private router: Router,
    private navigate: NavigationService
  ) {}
  async likeAsync(post: PostBase) {
    try {
      if (post.requestProfileIsInLikers == true) return;

      post.countLikes++;
      post.requestProfileIsInLikers = true;

      let success = await this.client.likeAsync(post.id);
      if (success == false) throw 'like success is false';
    } catch (error) {
      console.log(error);
      post.countLikes--;
      post.requestProfileIsInLikers = false;
    }
  }
  async unLikeAsync(post: PostBase) {
    try {
      if (post.requestProfileIsInLikers == false) return;

      post.countLikes--;
      post.requestProfileIsInLikers = false;

      let success = await this.client.unLikeAsync(post.id);
      if (success == false) throw 'unlike success is false';
    } catch (error) {
      console.log(error);

      post.countLikes++;
      post.requestProfileIsInLikers = true;
    }
  }
  async getLikers(postId: number, page: number): Promise<Profile[]> {
    return await this.client.getLikersAsync(postId, page);
  }
}
