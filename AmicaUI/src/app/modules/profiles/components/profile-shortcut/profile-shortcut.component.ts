import { Profile } from './../../../../models/Profile';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile-shortcut',
  templateUrl: './profile-shortcut.component.html',
  styleUrls: ['./profile-shortcut.component.scss'],
})
export class ProfileShortcutComponent implements OnInit {
  @Input() url: string | undefined;
  @Input() profile: Profile | undefined;
  constructor() {}

  ngOnInit(): void {}
}
