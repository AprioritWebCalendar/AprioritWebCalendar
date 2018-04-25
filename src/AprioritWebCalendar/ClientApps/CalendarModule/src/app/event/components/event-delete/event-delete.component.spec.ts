import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventDeleteComponent } from './event-delete.component';

describe('EventDeleteComponent', () => {
  let component: EventDeleteComponent;
  let fixture: ComponentFixture<EventDeleteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventDeleteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
