import { FormsModule } from '@angular/forms';
import { AppPrimaryRoutingModule } from 'src/app/app-primary-routing.module';
import { CommentsService } from './services/CommentsService';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddCommentFormComponent } from './widgets/add-comment-form/add-comment-form.component';
import { CommentsListComponent } from './widgets/comments-list/comments-list.component';
import { CommentComponent } from './widgets/comment/comment.component';
import { CommentsApiClient } from './clients/CommentsApiClient';
import { CommonComponentsModule } from '../common/common-components.module';

@NgModule({
  declarations: [
    AddCommentFormComponent,
    CommentsListComponent,
    CommentComponent,
  ],
  providers: [CommentsApiClient, CommentsService],
  exports: [AddCommentFormComponent, CommentsListComponent],
  imports: [
    CommonModule,
    AppPrimaryRoutingModule,
    CommonComponentsModule,
    FormsModule,
  ],
})
export class CommentsModule {}
