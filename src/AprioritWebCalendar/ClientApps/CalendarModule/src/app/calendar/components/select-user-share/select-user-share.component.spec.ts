import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectUserShareComponent } from './select-user-share.component';

describe('SelectUserShareComponent', () => {
  let component: SelectUserShareComponent;
  let fixture: ComponentFixture<SelectUserShareComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectUserShareComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectUserShareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
