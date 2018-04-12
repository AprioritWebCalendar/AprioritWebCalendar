import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarViewSwitcherComponent } from './calendar-view-switcher.component';

describe('CalendarViewSwitcherComponent', () => {
  let component: CalendarViewSwitcherComponent;
  let fixture: ComponentFixture<CalendarViewSwitcherComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalendarViewSwitcherComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarViewSwitcherComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
