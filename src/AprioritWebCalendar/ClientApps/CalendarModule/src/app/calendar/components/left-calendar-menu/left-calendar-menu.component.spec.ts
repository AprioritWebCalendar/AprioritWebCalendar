import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LeftCalendarMenuComponent } from './left-calendar-menu.component';

describe('LeftCalendarMenuComponent', () => {
  let component: LeftCalendarMenuComponent;
  let fixture: ComponentFixture<LeftCalendarMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LeftCalendarMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LeftCalendarMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
