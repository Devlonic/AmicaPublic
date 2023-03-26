import { FileParameter } from 'src/app/clients/common/FileParameter';

export interface PostCreateRequestDTO {
  title: string;
  images: FileParameter[];
}
