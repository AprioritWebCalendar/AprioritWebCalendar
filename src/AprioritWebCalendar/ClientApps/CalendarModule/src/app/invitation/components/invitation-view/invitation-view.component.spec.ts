import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InvitationViewComponent } from './invitation-view.component';

describe('InvitationViewComponent', () => {
  let component: InvitationViewComponent;
  let fixture: ComponentFixture<InvitationViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InvitationViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvitationViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
