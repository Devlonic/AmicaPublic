import { PostComment } from './../../../../models/Comment';
import { CommentsService } from './../../../comments/services/CommentsService';
import { FullPost } from 'src/app/models/FullPost';
import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { ApiException } from 'src/app/clients/common/ApiException';
import { NavigationService } from 'src/app/modules/common/services/NavigationService';
import { FollowersService } from 'src/app/modules/followers/services/FollowersService';
import { ProfilesService } from 'src/app/modules/profiles/services/ProfilesService';
import { PostsService } from '../../services/PostsService';

@Component({
  selector: 'app-full-post',
  templateUrl: './full-post.component.html',
  styleUrls: ['./full-post.component.scss'],
})
export class FullPostComponent implements OnChanges {
  post: FullPost | undefined;
  comments: PostComment[] | undefined;
  @Input() postId: number | undefined;
  constructor(
    private navigate: NavigationService,
    private profilesService: ProfilesService,
    private postsService: PostsService,
    private commentsService: CommentsService,
    private followersService: FollowersService
  ) {}

  // occurs when profile is changed by route (@Input() id: number changing)
  async ngOnChanges(changes: SimpleChanges): Promise<void> {
    if (this.postId !== undefined) {
      try {
        console.log(this.postId);
        this.post =
          (await this.postsService.getPostAsync(this.postId)) ?? undefined;

        if (this.post !== undefined) {
          this.comments = await this.commentsService.getCommentsByPostAsync(
            this.post,
            0
          );
          console.log(this.comments);
        }
      } catch (error: ApiException | any) {
        let e = error as ApiException;
        if (e.status == 404) this.navigate.toNotFound();
      }
    } else {
      this.navigate.toNotFound();
    }
  }
}
