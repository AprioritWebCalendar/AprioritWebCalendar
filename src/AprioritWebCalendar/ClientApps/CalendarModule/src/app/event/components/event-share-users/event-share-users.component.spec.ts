import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EventShareUsersComponent } from './event-share-users.component';

describe('EventShareUsersComponent', () => {
  let component: EventShareUsersComponent;
  let fixture: ComponentFixture<EventShareUsersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EventShareUsersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EventShareUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
