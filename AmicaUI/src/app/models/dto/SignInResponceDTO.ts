import { Profile } from './../Profile';
export interface SignInResponceDTO {
  isSignedIn: boolean;
  message: string;
  token: string;
  profile: Profile;
}
