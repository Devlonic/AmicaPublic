import { AccountsService } from './../../../accounts/services/AccountsService';
import { Profile } from './../../../../models/Profile';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-profile-header',
  templateUrl: './profile-header.component.html',
  styleUrls: ['./profile-header.component.scss'],
})
export class ProfileHeaderComponent implements OnInit {
  @Input() profile: Profile | undefined;
  @Input() isFollowProcessing: boolean = false;
  @Output() follow: EventEmitter<Profile> = new EventEmitter<Profile>();
  constructor(private accountsService: AccountsService) {}

  ngOnInit(): void {}

  onFollow() {
    this.follow.emit(this.profile);
  }
  onLogout() {
    this.accountsService.signOut();
  }
}
