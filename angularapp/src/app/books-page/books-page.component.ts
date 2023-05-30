import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { GetBookDto } from '../api.service';
import { BookItemComponent } from "../book-item/book-item.component";

@Component({
    selector: 'app-books-page',
    templateUrl: './books-page.component.html',
    styleUrls: ['./books-page.component.css'],
    standalone: true,
    imports: [
      BookItemComponent,
      CommonModule,
    ]
})
export class BooksPageComponent {
  constructor() {
    
  }
  @Input() books: GetBookDto[] | undefined
}
