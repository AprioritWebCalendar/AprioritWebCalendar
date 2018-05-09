import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SettingsTimezoneComponent } from './settings-timezone.component';

describe('SettingsTimezoneComponent', () => {
  let component: SettingsTimezoneComponent;
  let fixture: ComponentFixture<SettingsTimezoneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SettingsTimezoneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SettingsTimezoneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
