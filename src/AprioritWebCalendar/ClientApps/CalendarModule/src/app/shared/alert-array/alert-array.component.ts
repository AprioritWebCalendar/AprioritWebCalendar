import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-alert-array',
  templateUrl: './alert-array.component.html'
})
export class AlertArrayComponent {
  constructor() { }

  @Input()
  public alertType: string;

  @Input()
  public errors: string[];
}
