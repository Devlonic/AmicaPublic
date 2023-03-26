import { AppPrimaryRoutingModule } from 'src/app/app-primary-routing.module';
import { LikesApiClient } from './clients/LikesApiClient';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LikeButtonComponent } from './widgets/like-button/like-button.component';
import { LikesService } from './services/LikesService';
import { LikersLinkComponent } from './widgets/likers-link/likers-link.component';

@NgModule({
  declarations: [LikeButtonComponent, LikersLinkComponent],
  imports: [CommonModule, AppPrimaryRoutingModule],
  providers: [LikesService, LikesApiClient],
  exports: [LikeButtonComponent, LikersLinkComponent],
})
export class LikesModule {}
