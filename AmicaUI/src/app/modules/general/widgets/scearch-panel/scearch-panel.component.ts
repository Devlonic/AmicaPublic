import { ProfilesService } from './../../../profiles/services/ProfilesService';
import { NgModel } from '@angular/forms';
import {
  AfterViewChecked,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Profile } from 'src/app/models/Profile';

@Component({
  selector: 'app-scearch-panel',
  templateUrl: './scearch-panel.component.html',
  styleUrls: ['./scearch-panel.component.scss'],
})
export class ScearchPanelComponent implements OnInit {
  query: string | undefined;
  profiles: Profile[] | undefined;
  constructor(private profilesService: ProfilesService) {}

  ngOnInit(): void {}

  async onScearchAsync() {
    if (this.query == undefined) return;
    this.profiles = await this.profilesService.scearchProfilesByNickNameAsync(
      this.query
    );
    console.log(this.profiles);
  }
}
