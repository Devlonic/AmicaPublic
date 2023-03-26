import { PostComment } from './../../../../models/Comment';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss'],
})
export class CommentComponent implements OnInit {
  @Input() comment: PostComment | undefined;
  constructor() {}

  ngOnInit(): void {}
}
