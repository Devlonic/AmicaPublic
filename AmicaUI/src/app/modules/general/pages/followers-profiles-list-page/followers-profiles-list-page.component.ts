import { FollowersService } from 'src/app/modules/followers/services/FollowersService';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Profile } from 'src/app/models/Profile';

@Component({
  selector: 'app-followers-profiles-list-page',
  templateUrl: './followers-profiles-list-page.component.html',
  styleUrls: ['./followers-profiles-list-page.component.scss'],
})
export class FollowersProfilesListPageComponent implements OnInit {
  routePostId: number | undefined;
  routeStrategy: string | undefined;
  profiles: Profile[] | undefined;
  currentPage: number | undefined;
  constructor(
    private route: ActivatedRoute,
    private followersService: FollowersService
  ) {}

  private async loadProfilesAsync() {
    if (
      this.routePostId == undefined ||
      this.currentPage == undefined ||
      this.routeStrategy == undefined
    )
      return;
    switch (this.routeStrategy) {
      case 'followers': {
        this.profiles = await this.followersService.getFollowersAsync(
          this.routePostId,
          0
        );
        break;
      }
      case 'followings': {
        this.profiles = await this.followersService.getFollowingsAsync(
          this.routePostId,
          0
        );
        break;
      }
    }

    console.log(this.profiles);
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(async (p) => {
      this.profiles = undefined;
      this.currentPage = 0;

      this.routePostId = p['id'];
      this.routeStrategy = p['strategy'];
      await this.loadProfilesAsync();
    });
  }
}
