import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventSelectUserShareComponent } from './event-select-user-share.component';

describe('EventSelectUserShareComponent', () => {
  let component: EventSelectUserShareComponent;
  let fixture: ComponentFixture<EventSelectUserShareComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventSelectUserShareComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventSelectUserShareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
