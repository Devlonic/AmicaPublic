import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfilesListPageComponent } from './profiles-list-page.component';

describe('ProfilesListPageComponent', () => {
  let component: ProfilesListPageComponent;
  let fixture: ComponentFixture<ProfilesListPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProfilesListPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfilesListPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
