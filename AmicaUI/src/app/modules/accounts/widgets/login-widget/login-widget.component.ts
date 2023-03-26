import { NavigationService } from './../../../common/services/NavigationService';
import { AccountsService } from './../../services/AccountsService';
import { SignInRequestDTO } from './../../../../models/dto/SignInRequestDTO';
import {
  AfterViewChecked,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ApiException } from 'src/app/clients/common/ApiException';
import { NgModel } from '@angular/forms';

@Component({
  selector: 'app-login-widget',
  templateUrl: './login-widget.component.html',
  styleUrls: ['./login-widget.component.scss', './../../../../../styles.scss'],
})
export class LoginWidgetComponent implements OnInit, AfterViewChecked {
  @ViewChild('formError') formError: ElementRef | undefined;
  @ViewChild('formErrorText') formErrorText: ElementRef | undefined;
  signIn: SignInRequestDTO;

  errorMessage: string | undefined;
  processing: boolean = false;
  constructor(
    private accounts: AccountsService,
    private navigate: NavigationService
  ) {
    this.signIn = { login: '', password: '' };
  }
  ngAfterViewChecked(): void {}

  ngOnInit(): void {
    console.log('oninit login');
    if (this.accounts.isSignedIn()) {
      console.log('yes');

      this.navigate.toHome();
    }
  }

  removeError(): void {
    (this.formError?.nativeElement as HTMLElement).classList.remove('show');
  }
  showError(text: string): void {
    (this.formError?.nativeElement as HTMLElement).classList.add('show');
    const elem = this.formErrorText?.nativeElement as HTMLElement;
    elem.innerText = text;
  }

  async onLogin() {
    this.processing = true;
    console.log(this.signIn);
    try {
      var res = await this.accounts.signInAsync(this.signIn);
      if (res.isSignedIn == false) this.showError(res.message);
    } catch (error) {
      console.log(error);
      this.showError((error as ApiException).message);
    } finally {
      this.processing = false;
    }
  }
}
