import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FollowersProfilesListPageComponent } from './followers-profiles-list-page.component';

describe('FollowersProfilesListPageComponent', () => {
  let component: FollowersProfilesListPageComponent;
  let fixture: ComponentFixture<FollowersProfilesListPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FollowersProfilesListPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FollowersProfilesListPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
