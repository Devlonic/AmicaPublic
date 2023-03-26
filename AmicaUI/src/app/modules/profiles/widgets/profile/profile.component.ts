import { NavigationService } from './../../../common/services/NavigationService';
import { ApiException } from './../../../../clients/common/ApiException';
import { FollowersService } from './../../../followers/services/FollowersService';
import { PostsService } from './../../../posts/services/PostsService';
import { ProfilesService } from './../../services/ProfilesService';
import { ProfilePost } from './../../../../models/ProfilePost';
import { Profile } from './../../../../models/Profile';
import {
  Component,
  Input,
  OnInit,
  OnChanges,
  SimpleChanges,
} from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnChanges {
  profile: Profile | undefined;
  posts: ProfilePost[] | undefined = [];
  isLoading: boolean = false;
  isFollowingProcessing: boolean = false;
  @Input() nickName: string | undefined;
  constructor(
    private navigate: NavigationService,
    private profilesService: ProfilesService,
    private postsService: PostsService,
    private followersService: FollowersService
  ) {}
  async loadAndPushPostsAsync(page: number): Promise<void> {
    if (this.profile !== undefined) {
      let posts = await this.postsService.getPostsAsync(this.profile?.id, page);
      this.posts = this.posts?.concat(posts);
    }
  }

  // occurs when profile is changed by route (@Input() id: number changing)
  async ngOnChanges(changes: SimpleChanges): Promise<void> {
    if (this.nickName !== undefined) {
      try {
        console.log(this.nickName);
        this.profile = await this.profilesService.getProfileAsync(
          this.nickName
        );
        console.log(this.profile);
        this.posts = [];
        await this.loadAndPushPostsAsync(0);
      } catch (error: ApiException | any) {
        let e = error as ApiException;
        if (e.status == 404) this.navigate.toNotFound();
      }
    } else {
      this.navigate.toNotFound();
    }
  }
  async loadMorePostsHandler(page: number) {
    this.isLoading = true;
    await this.loadAndPushPostsAsync(page);
    this.isLoading = false;
  }
  async followHandler(profile: Profile) {
    console.log('followHandler');
    if (this.isFollowingProcessing == false) {
      this.isFollowingProcessing = true;

      if (profile.isRequesterFollowsProfile == false)
        await this.followersService.followAsync(profile);
      else if (profile.isRequesterFollowsProfile == true)
        await this.followersService.unFollowAsync(profile);

      this.isFollowingProcessing = false;
    }
  }
}
