import { PostBase } from 'src/app/models/PostBase';
import { LikesService } from './../../services/LikesService';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-like-button',
  templateUrl: './like-button.component.html',
  styleUrls: ['./like-button.component.scss'],
})
export class LikeButtonComponent implements OnInit {
  @Input() post: PostBase | undefined | any;
  constructor(private likesService: LikesService) {}

  ngOnInit(): void {}

  async onLikeClick(event: any) {
    if (this.post != undefined) {
      if (this.post.requestProfileIsInLikers == false)
        await this.likesService.likeAsync(this.post);
      else await this.likesService.unLikeAsync(this.post);
    }
  }
}
