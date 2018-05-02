import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarExportComponent } from './calendar-export.component';

describe('CalendarExportComponent', () => {
  let component: CalendarExportComponent;
  let fixture: ComponentFixture<CalendarExportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalendarExportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarExportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
