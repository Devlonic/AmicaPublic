import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-round-linked-image',
  templateUrl: './round-linked-image.component.html',
  styleUrls: ['./round-linked-image.component.scss'],
})
export class RoundLinkedImageComponent implements OnInit {
  @Input() style: any;
  @Input() imageUrl: string | undefined;
  constructor() {}

  ngOnInit(): void {}
}
