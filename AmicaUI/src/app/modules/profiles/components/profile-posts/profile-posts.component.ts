import { ProfilePost } from './../../../../models/ProfilePost';
import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { Profile } from 'src/app/models/Profile';

@Component({
  selector: 'app-profile-posts',
  templateUrl: './profile-posts.component.html',
  styleUrls: ['./profile-posts.component.scss'],
})
export class ProfilePostsComponent implements OnChanges {
  @Input() posts: ProfilePost[] | undefined;
  postRows: ProfilePost[][] = [];
  @Input() rowLength: number = 3;
  currentPage: number = 0;
  @Output() loadMorePostsEvent = new EventEmitter<number>();

  prevCount: number = 0;
  loadedMoreThanZero: boolean | undefined;
  @Input() isLoading: boolean | undefined;

  ngOnChanges(changes: SimpleChanges): void {
    if (this.posts == undefined) return;
    this.postRows = [];
    for (let i = 0; i < this.posts.length; ) {
      let row = [];
      for (let j = 0; j < this.rowLength; j++, i++) {
        const p = this.posts[i];
        if (p == undefined || p == null) continue;
        row[j] = p;
      }
      this.postRows.push(row);
    }
    if (this.posts.length - this.prevCount == 0)
      this.loadedMoreThanZero = false;
    else this.loadedMoreThanZero = true;

    this.prevCount = this.posts.length;
  }
  onLoadMorePosts() {
    this.loadMorePostsEvent.emit(++this.currentPage);
  }
}
