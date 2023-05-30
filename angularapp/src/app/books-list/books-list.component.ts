import { Component, OnInit, ViewChild } from '@angular/core';
import {MatTabsModule} from '@angular/material/tabs';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { GetBookDto, Client, SaveANewBookDto } from '../api.service';
import { CommonModule } from '@angular/common';
import { MatGridListModule } from '@angular/material/grid-list';
import { BookItemComponent } from "../book-item/book-item.component";
import { BooksPageComponent } from "../books-page/books-page.component";
import { Select, State, Store } from '@ngxs/store';
import { BooksListState, LoadBooksList, SelectedBookState, ClearBook } from '../books.actions';
import { Observable } from 'rxjs';
import { EditBookComponent } from '../edit-book/edit-book.component';

enum Order {
  Title = "title",
  Author = "author",
}

@Component({
    selector: 'app-books-list',
    templateUrl: './books-list.component.html',
    styleUrls: ['./books-list.component.css'],
    standalone: true,
    imports: [
        MatTabsModule,
        MatProgressSpinnerModule,
        CommonModule,
        MatGridListModule,
        BookItemComponent,
        BooksPageComponent,
        BooksPageComponent
    ]
})
export class BooksListComponent implements OnInit {
  constructor(
    private api_client: Client,
    private readonly store: Store) {  
        
  }

  @Select(BooksListState.getBooksList) 
  allBooks$?: Observable<GetBookDto[]>

  recommended: GetBookDto[] | undefined = undefined

  order: Order = Order.Title

  book: SaveANewBookDto | undefined;

  ngOnInit(): void {
    this.getAllBooks()
    // this.allBooks$?.subscribe(x=> {
    //   this.recommended = x
    //   console.log('hello' + x);
    // })
  }

  getAllBooks(){
    this.api_client
    .booksAll(this.order)
    .subscribe(x=> {
      console.log(x);
      this.store.dispatch(new LoadBooksList(x))
      this.store.dispatch(new ClearBook(EditBookComponent.getBookTemplate()))
    })
  }

  getRecommened(){
    this.api_client.recomended(undefined)
    .subscribe(x=> {
      this.recommended = x
    })
    console.log('fetched recommended');
  }


}
