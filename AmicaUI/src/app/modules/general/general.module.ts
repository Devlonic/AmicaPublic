import { FormsModule } from '@angular/forms';
import { CommonComponentsModule } from './../common/common-components.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GeneralPageComponent } from './pages/general-page/general-page.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { AppPrimaryRoutingModule } from 'src/app/app-primary-routing.module';
import { GeneralNavBarComponent } from './widgets/general-nav-bar/general-nav-bar.component';
import { PostsModule } from '../posts/posts.module';
import { ProfilePageComponent } from './pages/profile-page/profile-page.component';
import { ProfilesModule } from '../profiles/profiles.module';
import { ScearchPanelComponent } from './widgets/scearch-panel/scearch-panel.component';
import { ScearchPageComponent } from './pages/scearch-page/scearch-page.component';
import { PostPageComponent } from './pages/post-page/post-page.component';
import { CreatePostPageComponent } from './pages/create-post-page/create-post-page.component';
import { FollowersProfilesListPageComponent } from './pages/followers-profiles-list-page/followers-profiles-list-page.component';
import { ProfilesListPageComponent } from './pages/profiles-list-page/profiles-list-page.component';

@NgModule({
  declarations: [
    GeneralPageComponent,
    HomePageComponent,
    ProfilePageComponent,
    GeneralNavBarComponent,
    ScearchPanelComponent,
    ScearchPageComponent,
    PostPageComponent,
    CreatePostPageComponent,
    ProfilesListPageComponent,
    FollowersProfilesListPageComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    AppPrimaryRoutingModule,
    PostsModule,
    ProfilesModule,
    CommonComponentsModule,
  ],
})
export class GeneralModule {}
