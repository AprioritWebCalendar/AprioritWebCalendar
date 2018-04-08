import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedUsersListComponent } from './shared-users-list.component';

describe('SharedUsersListComponent', () => {
  let component: SharedUsersListComponent;
  let fixture: ComponentFixture<SharedUsersListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SharedUsersListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SharedUsersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
