import { FileParameter } from 'src/app/clients/common/FileParameter';

export interface SignUpRequestDTO {
  email: string;
  fullName: string;
  nickname: string;
  password: string;
  profilePhoto: FileParameter;
  captchaV2: string;
}
