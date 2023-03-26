import { FormsModule } from '@angular/forms';
import { NavigationService } from './services/NavigationService';
import { LoadingButtonComponent } from './components/loading-button.components';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SpinnerComponent } from './components/spinner.component';
import { LinkedFooterComponent } from './components/linked-footer/linked-footer.component';
import { RoundLinkedImageComponent } from './components/round-linked-image/round-linked-image.component';

@NgModule({
  declarations: [
    SpinnerComponent,
    LoadingButtonComponent,
    LinkedFooterComponent,
    RoundLinkedImageComponent,
  ],
  imports: [CommonModule, FormsModule],
  exports: [
    SpinnerComponent,
    LoadingButtonComponent,
    RoundLinkedImageComponent,
  ],
})
export class CommonComponentsModule {}
