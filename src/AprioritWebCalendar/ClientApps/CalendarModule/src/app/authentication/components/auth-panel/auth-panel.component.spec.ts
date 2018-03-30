import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthPanelComponent } from './auth-panel.component';

describe('AuthPanelComponent', () => {
  let component: AuthPanelComponent;
  let fixture: ComponentFixture<AuthPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuthPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
