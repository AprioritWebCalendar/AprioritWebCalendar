import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InvitationsIncomingComponent } from './invitations-incoming.component';

describe('InvitationsIncomingComponent', () => {
  let component: InvitationsIncomingComponent;
  let fixture: ComponentFixture<InvitationsIncomingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InvitationsIncomingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvitationsIncomingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
