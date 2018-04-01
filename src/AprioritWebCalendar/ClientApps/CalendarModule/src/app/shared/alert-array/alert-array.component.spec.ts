import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AlertArrayComponent } from './alert-array.component';

describe('AlertArrayComponent', () => {
  let component: AlertArrayComponent;
  let fixture: ComponentFixture<AlertArrayComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AlertArrayComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AlertArrayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
