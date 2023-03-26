import { PostBase } from './PostBase';
import { Profile } from './Profile';
export interface FeedPost extends PostBase {
  title: string;
  id_Author: number;
  author: Profile;
  dateCreated: string;
}
