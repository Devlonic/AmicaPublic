import { ApiException } from 'src/app/clients/common/ApiException';
import { PostsService } from './../../services/PostsService';
import { FeedPost } from './../../../../models/FeedPost';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-feed-posts-list',
  templateUrl: './feed-posts-list.component.html',
  styleUrls: ['./feed-posts-list.component.scss'],
})
export class FeedPostsListComponent implements OnInit {
  posts: FeedPost[] | undefined;
  loadingError: string | null = null;
  constructor(private service: PostsService) {}

  async ngOnInit(): Promise<void> {
    try {
      this.posts = await this.service.getFeedsAsync(0);
    } catch (error) {
      this.loadingError = (error as ApiException).message;
    }
  }
}
