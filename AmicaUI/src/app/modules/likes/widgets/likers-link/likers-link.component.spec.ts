import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LikersLinkComponent } from './likers-link.component';

describe('LikersLinkComponent', () => {
  let component: LikersLinkComponent;
  let fixture: ComponentFixture<LikersLinkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LikersLinkComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LikersLinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
