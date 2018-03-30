import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html'
})
export class AlertComponent {
  constructor() { }

  @Input()
  public alertType: string;

  @Input()
  public innerText: string;
}
