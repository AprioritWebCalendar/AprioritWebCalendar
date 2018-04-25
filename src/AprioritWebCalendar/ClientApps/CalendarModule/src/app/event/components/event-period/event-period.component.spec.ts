import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventPeriodComponent } from './event-period.component';

describe('EventPeriodComponent', () => {
  let component: EventPeriodComponent;
  let fixture: ComponentFixture<EventPeriodComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventPeriodComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventPeriodComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
