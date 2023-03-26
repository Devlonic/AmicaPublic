import { Profile } from './../../../../models/Profile';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-general-nav-bar',
  templateUrl: './general-nav-bar.component.html',
  styleUrls: ['./general-nav-bar.component.scss'],
})
export class GeneralNavBarComponent implements OnInit {
  @Input() profile: Profile | undefined;
  constructor() {}

  ngOnInit(): void {}
}
