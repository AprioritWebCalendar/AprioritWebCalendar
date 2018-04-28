import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InvitationsIncomingButtonComponent } from './invitations-incoming-button.component';

describe('InvitationsIncomingButtonComponent', () => {
  let component: InvitationsIncomingButtonComponent;
  let fixture: ComponentFixture<InvitationsIncomingButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InvitationsIncomingButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvitationsIncomingButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
