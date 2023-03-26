import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileShortcutComponent } from './profile-shortcut.component';

describe('ProfileShortcutComponent', () => {
  let component: ProfileShortcutComponent;
  let fixture: ComponentFixture<ProfileShortcutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProfileShortcutComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfileShortcutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
