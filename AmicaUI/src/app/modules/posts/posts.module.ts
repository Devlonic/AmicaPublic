import { LikesModule } from './../likes/likes.module';
import { CommonComponentsModule } from './../common/common-components.module';
import { PostsService } from './services/PostsService';
import { PostsApiClient } from './clients/PostsApiClient';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FeedPostsListComponent } from './widgets/feed-posts-list/feed-posts-list.component';
import { AppPrimaryRoutingModule } from 'src/app/app-primary-routing.module';
import { FeedPostComponent } from './widgets/feed-post/feed-post.component';
import { FullPostComponent } from './widgets/full-post/full-post.component';
import { CommentsModule } from '../comments/comments.module';
import { ProfilePostComponent } from './widgets/profile-post/profile-post.component';
import { CreateNewPostComponent } from './widgets/create-new-post/create-new-post.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    FeedPostComponent,
    FeedPostsListComponent,
    FullPostComponent,
    ProfilePostComponent,
    CreateNewPostComponent,
  ],
  providers: [PostsApiClient, PostsService],
  exports: [
    FeedPostComponent,
    FeedPostsListComponent,
    FullPostComponent,
    ProfilePostComponent,
    CreateNewPostComponent,
  ],
  imports: [
    CommonModule,
    AppPrimaryRoutingModule,
    CommonComponentsModule,
    CommentsModule,
    FormsModule,
    LikesModule,
  ],
})
export class PostsModule {}
