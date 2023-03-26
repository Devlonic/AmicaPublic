import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedPostComponent } from './feed-post.component';

describe('FeedPostComponent', () => {
  let component: FeedPostComponent;
  let fixture: ComponentFixture<FeedPostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FeedPostComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FeedPostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
