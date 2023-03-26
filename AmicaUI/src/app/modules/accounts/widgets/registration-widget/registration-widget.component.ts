import { AMICA_CAPTCHA_V2_KEY } from './../../../../clients/common/AMICA_CAPTCHA_V2_KEY';
import { SignUpResponceDTO } from 'src/app/models/dto/SignUpResponceDTO';
import { SignUpRequestDTO } from './../../../../models/dto/SignUpRequestDTO';
import {
  Component,
  ElementRef,
  Inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ApiException } from 'src/app/clients/common/ApiException';
import { SignInRequestDTO } from 'src/app/models/dto/SignInRequestDTO';
import { AccountsService } from '../../services/AccountsService';
import { NgModel } from '@angular/forms';
@Component({
  selector: 'app-registration-widget',
  templateUrl: './registration-widget.component.html',
  styleUrls: [
    './registration-widget.component.scss',
    './../../../../../styles.scss',
  ],
})
export class RegistrationWidgetComponent implements OnInit {
  @ViewChild('formError') formError: ElementRef | undefined;
  @ViewChild('formErrorText') formErrorText: ElementRef | undefined;

  request: SignUpRequestDTO;

  errorMessage: string | undefined;
  processing: boolean = false;

  captchaKey: string = '';

  private grecaptcha: any;
  constructor(
    private accounts: AccountsService,
    @Inject(AMICA_CAPTCHA_V2_KEY) captchaKey?: string
  ) {
    if (captchaKey != undefined) this.captchaKey = captchaKey;
    this.request = {
      email: '',
      fullName: '',
      nickname: '',
      password: '',
      profilePhoto: {
        data: '',
        fileName: '',
      },
      captchaV2: '',
    };
  }

  ngOnInit(): void {}

  fileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.request.profilePhoto.fileName = file.name;
      this.request.profilePhoto.data = file;
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

  captchaError(e: any) {
    this.showError('error' + e);
  }
  captchaResolved(e: any) {
    console.log('captcha resolved: ' + e);
    if (e != null && e != undefined) this.request.captchaV2 = e;
  }

  async onRegister() {
    this.processing = true;
    console.log(this.request);
    try {
      var res = await this.accounts.signUpAsync(this.request, true);
      if (res.isSignedUp == false) this.showError(res.message);
    } catch (error) {
      console.log(error);
      this.showError(
        (JSON.parse((error as ApiException).response) as SignUpResponceDTO)
          .message
      );
    } finally {
      this.processing = false;
    }
  }
}
