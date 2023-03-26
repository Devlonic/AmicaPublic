import { FeedPost } from './../../../../models/FeedPost';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-feed-post',
  templateUrl: './feed-post.component.html',
  styleUrls: ['./feed-post.component.scss'],
})
export class FeedPostComponent implements OnInit {
  @Input() post: FeedPost | undefined;
  constructor() {}

  ngOnInit(): void {}
}
