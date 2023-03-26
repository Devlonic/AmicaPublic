import { PostsModule } from './../posts/posts.module';
import { CommonComponentsModule } from './../common/common-components.module';
import { NavigationService } from './../common/services/NavigationService';
import { ProfilesService } from './services/ProfilesService';
import { ProfilesApiClient } from './clients/ProfilesApiClient';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppPrimaryRoutingModule } from 'src/app/app-primary-routing.module';
import { ProfileComponent } from './widgets/profile/profile.component';
import { ProfileHeaderComponent } from './components/profile-header/profile-header.component';
import { ProfilePostsComponent } from './components/profile-posts/profile-posts.component';
import { ProfilesListComponent } from './widgets/profiles-list/profiles-list.component';
import { ProfileShortcutComponent } from './components/profile-shortcut/profile-shortcut.component';

@NgModule({
  declarations: [
    ProfileComponent,
    ProfileHeaderComponent,
    ProfilePostsComponent,
    ProfilesListComponent,
    ProfileShortcutComponent,
  ],
  providers: [ProfilesApiClient, ProfilesService],
  imports: [
    CommonModule,
    AppPrimaryRoutingModule,
    CommonComponentsModule,
    PostsModule,
  ],
  exports: [ProfileComponent, ProfilesListComponent],
})
export class ProfilesModule {}
