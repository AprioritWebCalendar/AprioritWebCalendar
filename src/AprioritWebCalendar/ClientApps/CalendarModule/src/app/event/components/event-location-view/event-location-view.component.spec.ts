import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventLocationViewComponent } from './event-location-view.component';

describe('EventLocationViewComponent', () => {
  let component: EventLocationViewComponent;
  let fixture: ComponentFixture<EventLocationViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventLocationViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventLocationViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
