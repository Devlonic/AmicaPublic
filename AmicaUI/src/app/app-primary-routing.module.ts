import { FollowersProfilesListPageComponent } from './modules/general/pages/followers-profiles-list-page/followers-profiles-list-page.component';
import { ProfilesListComponent } from './modules/profiles/widgets/profiles-list/profiles-list.component';
import { CreatePostPageComponent } from './modules/general/pages/create-post-page/create-post-page.component';
import { CreateNewPostComponent } from './modules/posts/widgets/create-new-post/create-new-post.component';
import { PostPageComponent } from './modules/general/pages/post-page/post-page.component';
import { ScearchPageComponent } from './modules/general/pages/scearch-page/scearch-page.component';
import { ProfilePageComponent } from './modules/general/pages/profile-page/profile-page.component';
import { HomePageComponent } from './modules/general/pages/home-page/home-page.component';
import { RegistrationPageComponent } from './modules/accounts/pages/registration-page/registration-page.component';
import { LoginPageComponent } from './modules/accounts/pages/login-page/login-page.component';
import { GeneralPageComponent } from './modules/general/pages/general-page/general-page.component';
import { AccountsPageComponent } from './modules/accounts/pages/accounts-page/accounts-page.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProfilesListPageComponent } from './modules/general/pages/profiles-list-page/profiles-list-page.component';

export const routes: Routes = [
  // /accounts
  {
    path: 'accounts',
    component: AccountsPageComponent,
    children: [
      { path: '', component: LoginPageComponent },
      { path: 'login', component: LoginPageComponent },
      { path: 'registration', component: RegistrationPageComponent },
    ],
  },
  // /
  {
    path: '',
    component: GeneralPageComponent,
    children: [
      { path: '', component: HomePageComponent },
      { path: 'profiles', component: ProfilePageComponent },
      { path: 'scearch', component: ScearchPageComponent },
      { path: 'posts', component: PostPageComponent },
      { path: 'createPost', component: CreatePostPageComponent },
      { path: 'likers', component: ProfilesListPageComponent },
      { path: 'followers', component: FollowersProfilesListPageComponent },
    ],
  },
  { path: '**', redirectTo: '/' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppPrimaryRoutingModule {}
