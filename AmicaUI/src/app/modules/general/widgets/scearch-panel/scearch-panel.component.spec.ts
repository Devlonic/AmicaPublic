import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScearchPanelComponent } from './scearch-panel.component';

describe('ScearchPanelComponent', () => {
  let component: ScearchPanelComponent;
  let fixture: ComponentFixture<ScearchPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScearchPanelComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ScearchPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
