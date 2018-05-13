import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SettingsTelegramComponent } from './settings-telegram.component';

describe('SettingsTelegramComponent', () => {
  let component: SettingsTelegramComponent;
  let fixture: ComponentFixture<SettingsTelegramComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SettingsTelegramComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SettingsTelegramComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
