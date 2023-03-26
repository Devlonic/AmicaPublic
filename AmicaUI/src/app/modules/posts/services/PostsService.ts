import { PostCreateResponceDTO } from './../../../models/dto/PostCreateResponceDTO';
import { PostCreateRequestDTO } from './../../../models/dto/PostCreateRequestDTO';
import { ProfilePost } from './../../../models/ProfilePost';
import { FeedPost } from './../../../models/FeedPost';
import { PostsApiClient } from './../clients/PostsApiClient';
import { NavigationService } from './../../common/services/NavigationService';
import { Injectable } from '@angular/core';
import { FullPost } from 'src/app/models/FullPost';

@Injectable({
  providedIn: 'root',
})
export class PostsService {
  constructor(
    private client: PostsApiClient,
    private navigate: NavigationService
  ) {}

  public async getFeedsAsync(page: number): Promise<FeedPost[]> {
    var res = await this.client.getFeedsAsync(page);
    return res;
  }
  public async getPostsAsync(
    profile_id: number,
    page: number
  ): Promise<ProfilePost[]> {
    var res = await this.client.getByProfileAsync(profile_id, page);
    return res;
  }
  public async getPostAsync(postId: number): Promise<FullPost | null> {
    var res = await this.client.getByIdAsync(postId);
    return res;
  }
  public async publishPostAsync(
    req: PostCreateRequestDTO
  ): Promise<PostCreateResponceDTO> {
    var res = await this.client.createPostAsync(req.title, req.images);
    return res;
  }
}
