import { Component, Input, OnInit } from '@angular/core';
import { Profile } from 'src/app/models/Profile';

@Component({
  selector: 'app-profiles-list',
  templateUrl: './profiles-list.component.html',
  styleUrls: ['./profiles-list.component.scss'],
})
export class ProfilesListComponent implements OnInit {
  @Input() profiles: Profile[] | undefined;
  constructor() {}

  ngOnInit(): void {}
}
