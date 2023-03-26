import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneralPageComponent } from './general-page.component';

describe('GeneralPageComponent', () => {
  let component: GeneralPageComponent;
  let fixture: ComponentFixture<GeneralPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GeneralPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GeneralPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
