import { Picture } from './Picture';
import { PostBase } from './PostBase';
import { Profile } from './Profile';
export interface FullPost extends PostBase {
  title: string;
  iD_Author: number;
  author: Profile;
  dateCreated: string;
  images: Picture[];
}
