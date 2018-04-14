import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEventButtonComponent } from './add-event-button.component';

describe('AddEventButtonComponent', () => {
  let component: AddEventButtonComponent;
  let fixture: ComponentFixture<AddEventButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddEventButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEventButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
