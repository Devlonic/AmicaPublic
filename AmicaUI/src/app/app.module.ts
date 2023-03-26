import { FollowersModule } from './modules/followers/followers.module';
import { ProfilesModule } from './modules/profiles/profiles.module';
import { RouterModule } from '@angular/router';
import { GeneralModule } from './modules/general/general.module';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppPrimaryRoutingModule } from './app-primary-routing.module';
import { AppComponent } from './app.component';
import { AccountsModule } from './modules/accounts/accounts.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    ProfilesModule,
    BrowserModule,
    AppPrimaryRoutingModule,
    AccountsModule,
    FollowersModule,
    GeneralModule,
    RouterModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
