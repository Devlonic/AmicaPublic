import { Profile } from 'src/app/models/Profile';
import { AccountsService } from './../../../accounts/services/AccountsService';
import { NavigationService } from 'src/app/modules/common/services/NavigationService';
import { ApiException } from 'src/app/clients/common/ApiException';
import { PostsService } from './../../services/PostsService';
import { FileParameter } from 'src/app/clients/common/FileParameter';
import { FormInputModel } from 'src/app/models/dto/FormInputModel';
import { PostCreateRequestDTO } from './../../../../models/dto/PostCreateRequestDTO';
import { FullPost } from './../../../../models/FullPost';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-create-new-post',
  templateUrl: './create-new-post.component.html',
  styleUrls: ['./create-new-post.component.scss'],
})
export class CreateNewPostComponent implements OnInit {
  @ViewChild('photoPreview') photoPreview: ElementRef | undefined;
  error: string | undefined;
  req: PostCreateRequestDTO = {
    title: '',
    images: [],
  };
  creator: Profile | undefined;

  constructor(
    private postsService: PostsService,
    private navigation: NavigationService,
    private accountService: AccountsService
  ) {}

  ngOnInit(): void {
    this.creator = this.accountService.getCurrentProfile();
  }

  async onCreatePost(event: any) {
    try {
      let responce = await this.postsService.publishPostAsync(this.req);
      console.log(responce);
      let postId = responce?.id;
      this.navigation.toPost(postId);
    } catch (error) {
      let e = error as ApiException;
      this.error = e.response;
    } finally {
      event?.preventDefault();
    }
  }
  fileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      let img: FileParameter = {
        fileName: '',
        data: null,
      };
      img.fileName = file.name;
      img.data = file;
      this.req.images[0] = img;
      this.showImage(img);
    }
  }
  showImage(file: FileParameter) {
    console.log(file);
    let native = this.photoPreview?.nativeElement as HTMLImageElement;
    native.src = URL.createObjectURL(file.data);
  }
}
