import { Profile } from './Profile';

export interface PostComment {
  id: number;
  text: string;
  date: string;
  author: Profile;
}
