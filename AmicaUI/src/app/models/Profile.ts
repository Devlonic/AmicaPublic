export interface Profile {
  id: number;
  profilePhoto: string;
  fullName: string;
  nickName: string;
  countPosts: number;
  countFollowers: number;
  countFollowing: number;
  isRequesterOwnsProfile: boolean;
  isRequesterFollowsProfile: boolean;
}
