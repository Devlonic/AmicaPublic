import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class NavigationService {
  constructor(private router: Router) {}

  public toHome() {
    this.router.navigate(['/']);
  }
  public toLogin() {
    this.router.navigate(['/accounts/login']);
  }
  public toRegistration() {
    this.router.navigate(['/accounts/registration']);
  }
  public toNotFound() {
    this.router.navigate(['/404']);
  }
  public toPost(postId: number) {
    this.router.navigate(['/posts'], { queryParams: { id: postId } });
  }
}
