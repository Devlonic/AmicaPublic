import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-spinner',
  template: ` <div
    *ngIf="rotating == true"
    class="spinner-border text-white"
    role="status"
  >
    <span class="visually-hidden">Loading...</span>
  </div>`,
})
export class SpinnerComponent implements OnInit {
  @Input() public rotating: boolean = false;
  constructor() {}
  ngOnInit(): void {}
}
