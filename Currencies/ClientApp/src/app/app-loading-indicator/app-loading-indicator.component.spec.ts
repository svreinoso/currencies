import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppLoadingIndicatorComponent } from './app-loading-indicator.component';

describe('AppLoadingIndicatorComponent', () => {
  let component: AppLoadingIndicatorComponent;
  let fixture: ComponentFixture<AppLoadingIndicatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppLoadingIndicatorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppLoadingIndicatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
