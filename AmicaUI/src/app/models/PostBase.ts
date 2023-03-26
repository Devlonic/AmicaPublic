export abstract class PostBase {
  id: number = -1;
  countLikes: number = -1;
  countComments: number | undefined;
  primaryImage: string | undefined;
  requestProfileIsInLikers: boolean = false;
}
