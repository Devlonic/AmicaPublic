import { Profile } from './../../../../models/Profile';
import { NavigationService } from './../../../common/services/NavigationService';
import { AccountsService } from './../../../accounts/services/AccountsService';
import { AfterViewChecked, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-general-page',
  templateUrl: './general-page.component.html',
  styleUrls: ['./general-page.component.scss'],
})
export class GeneralPageComponent implements OnInit {
  currentAuthProfile: Profile | undefined;
  constructor(
    private accounts: AccountsService,
    private navigate: NavigationService
  ) {}
  ngOnInit(): void {
    if (!this.accounts.isSignedIn()) {
      this.navigate.toLogin();
    } else {
      this.currentAuthProfile = this.accounts.getCurrentProfile();
    }
  }
}
