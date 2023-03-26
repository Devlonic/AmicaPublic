import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoundLinkedImageComponent } from './round-linked-image.component';

describe('RoundLinkedImageComponent', () => {
  let component: RoundLinkedImageComponent;
  let fixture: ComponentFixture<RoundLinkedImageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RoundLinkedImageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoundLinkedImageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
