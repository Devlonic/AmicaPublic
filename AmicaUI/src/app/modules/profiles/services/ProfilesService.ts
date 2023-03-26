import { ProfilesApiClient } from '../clients/ProfilesApiClient';
import { FeedPost } from '../../../models/FeedPost';
import { NavigationService } from '../../common/services/NavigationService';
import { Injectable } from '@angular/core';
import { Profile } from 'src/app/models/Profile';

@Injectable({
  providedIn: 'root',
})
export class ProfilesService {
  constructor(
    private client: ProfilesApiClient,
    private navigate: NavigationService
  ) {}

  public async getProfileAsync(profile_nickname: string): Promise<Profile> {
    var res = await this.client.getProfileAsync(profile_nickname);
    return res;
  }
  public async getProfileByIdAsync(id: number): Promise<Profile> {
    var res = await this.client.getProfileByIdAsync(id);
    return res;
  }
  public async scearchProfilesByNickNameAsync(
    part: string
  ): Promise<Profile[]> {
    var res = await this.client.scearchProfilesByNickNameAsync(part);
    return res;
  }
}
