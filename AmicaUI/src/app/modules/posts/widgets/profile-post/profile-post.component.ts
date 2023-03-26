import { ProfilePost } from './../../../../models/ProfilePost';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile-post',
  templateUrl: './profile-post.component.html',
  styleUrls: ['./profile-post.component.scss'],
})
export class ProfilePostComponent implements OnInit {
  @Input() post: ProfilePost | undefined;
  constructor() {}

  ngOnInit(): void {}
}
