import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventMoveComponent } from './event-move.component';

describe('EventMoveComponent', () => {
  let component: EventMoveComponent;
  let fixture: ComponentFixture<EventMoveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventMoveComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventMoveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
