import { PostBase } from 'src/app/models/PostBase';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-likers-link',
  templateUrl: './likers-link.component.html',
  styleUrls: ['./likers-link.component.scss'],
})
export class LikersLinkComponent implements OnInit {
  @Input() post: PostBase | undefined;
  constructor() {}

  ngOnInit(): void {}
}
