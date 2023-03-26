import { FollowersApiClient } from './../clients/FollowersApiClient';
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

@Injectable({
  providedIn: 'root',
})
export class FollowersService {
  constructor(
    private client: FollowersApiClient,
    private cookie: CookieService,
    private router: Router,
    private navigate: NavigationService
  ) {}
  async followAsync(profile: Profile) {
    await this.client.followAsync(profile);
  }
  async unFollowAsync(profile: Profile) {
    await this.client.unFollowAsync(profile);
  }

  async getFollowersAsync(profileId: number, page: number): Promise<Profile[]> {
    return await this.client.getFollowersAsync(profileId, page);
  }
  async getFollowingsAsync(
    profileId: number,
    page: number
  ): Promise<Profile[]> {
    return await this.client.getFollowingsAsync(profileId, page);
  }
}
