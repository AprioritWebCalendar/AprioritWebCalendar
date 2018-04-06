import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main-screen',
  templateUrl: './main-screen.component.html'
})
export class MainScreenComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  loadEvents(ids: Number[]) {
    console.log(ids.join(', '));
  }

}
