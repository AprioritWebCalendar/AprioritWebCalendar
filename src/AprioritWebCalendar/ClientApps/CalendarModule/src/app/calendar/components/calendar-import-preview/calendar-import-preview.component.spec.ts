import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarImportPreviewComponent } from './calendar-import-preview.component';

describe('CalendarImportPreviewComponent', () => {
  let component: CalendarImportPreviewComponent;
  let fixture: ComponentFixture<CalendarImportPreviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalendarImportPreviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarImportPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
