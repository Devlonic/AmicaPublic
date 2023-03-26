import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';

@Component({
  selector: 'app-loading-button',
  styleUrls: ['./styles.scss'],
  template: `
    <button
      [disabled]="this.canClick == false"
      type="submit"
      [ngClass]="{
        'btn p-2 m-4': true,
        'btn-primary': this.color == 'btn-primary',
        'btn-secondary': this.color == 'btn-secondary'
      }"
    >
      <span *ngIf="isLoading == false">{{ this.label }}</span>
      <app-spinner [rotating]="this.isLoading"></app-spinner>
    </button>
  `,
})
export class LoadingButtonComponent implements OnInit {
  @Input() isLoading: boolean = false;
  @Input() label: string | undefined;
  @Input() color: string | undefined = 'btn-primary';
  @Input() canClick: boolean = true;
  constructor() {}

  ngOnInit(): void {}
}
