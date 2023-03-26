import { Component, Input, OnInit } from '@angular/core';
import { PostComment } from 'src/app/models/Comment';

@Component({
  selector: 'app-comments-list',
  templateUrl: './comments-list.component.html',
  styleUrls: ['./comments-list.component.scss'],
})
export class CommentsListComponent implements OnInit {
  @Input() comments: PostComment[] | undefined;
  constructor() {}

  ngOnInit(): void {}
}
