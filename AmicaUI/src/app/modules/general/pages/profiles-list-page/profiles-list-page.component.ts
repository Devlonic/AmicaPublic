import { LikesService } from '../../../likes/services/LikesService';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Profile } from 'src/app/models/Profile';

@Component({
  selector: 'app-profiles-list-page',
  templateUrl: './profiles-list-page.component.html',
  styleUrls: ['./profiles-list-page.component.scss'],
})
export class ProfilesListPageComponent implements OnInit {
  routePostId: number | undefined;
  profiles: Profile[] | undefined;
  currentPage: number | undefined;
  constructor(
    private route: ActivatedRoute,
    private likesService: LikesService
  ) {}

  private async loadProfilesAsync() {
    if (this.routePostId == undefined || this.currentPage == undefined) return;
    this.profiles = await this.likesService.getLikers(
      this.routePostId,
      this.currentPage
    );
    console.log(this.profiles);
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(async (p) => {
      this.profiles = undefined;
      this.currentPage = 0;

      this.routePostId = p['id'];
      await this.loadProfilesAsync();
    });
  }
}
