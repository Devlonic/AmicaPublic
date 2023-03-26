import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScearchPageComponent } from './scearch-page.component';

describe('ScearchPageComponent', () => {
  let component: ScearchPageComponent;
  let fixture: ComponentFixture<ScearchPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScearchPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ScearchPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
