import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventShareComponent } from './event-share.component';

describe('EventShareComponent', () => {
  let component: EventShareComponent;
  let fixture: ComponentFixture<EventShareComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventShareComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventShareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
