import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventLocationComponent } from './event-location.component';

describe('EventLocationComponent', () => {
  let component: EventLocationComponent;
  let fixture: ComponentFixture<EventLocationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventLocationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventLocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
