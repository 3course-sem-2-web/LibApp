import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  template: `
    <p class="mat-headline-2" style="text-align: center;">
      Library Application
    </p>
    <p class="mat-subtitle-1" style="text-align: center;">
      Welcome to the home page
    </p>
  `,
  styles: [
  ]
})
export class HomeComponent {
  title: string = "Home"
}
