import { AccountsService } from './../../../accounts/services/AccountsService';
import { PostComment } from './../../../../models/Comment';
import { FeedPost } from './../../../../models/FeedPost';
import { FullPost } from 'src/app/models/FullPost';
import { PostBase } from 'src/app/models/PostBase';
import { CommentsService } from './../../services/CommentsService';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-add-comment-form',
  templateUrl: './add-comment-form.component.html',
  styleUrls: ['./add-comment-form.component.scss'],
})
export class AddCommentFormComponent implements OnInit {
  commentText: string = '';
  @Input() post: PostBase | FullPost | FeedPost | undefined;
  @Input() currentComments: PostComment[] | undefined;
  constructor(
    private commentsService: CommentsService,
    private accountsService: AccountsService
  ) {}

  ngOnInit(): void {}

  async onCreateComment() {
    let comment = await this.commentsService.createCommentToPostAsync(
      this.post,
      this.commentText
    );
    let currentProfile = this.accountsService.getCurrentProfile();

    if (
      comment != null &&
      comment != undefined &&
      currentProfile != undefined
    ) {
      this.commentText = '';
      comment.author = currentProfile;
      this.currentComments?.unshift(comment);
    }
  }
}
