import { FollowersService } from './services/FollowersService';
import { FollowersApiClient } from './clients/FollowersApiClient';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppPrimaryRoutingModule } from 'src/app/app-primary-routing.module';
import { CommonComponentsModule } from '../common/common-components.module';

@NgModule({
  declarations: [],
  providers: [FollowersApiClient, FollowersService],
  imports: [CommonModule, AppPrimaryRoutingModule, CommonComponentsModule],
})
export class FollowersModule {}
