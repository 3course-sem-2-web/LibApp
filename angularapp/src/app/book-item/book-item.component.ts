import { Component, Input, ViewChild, Output, EventEmitter, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { GetBookDto, Client } from '../api.service';
import { Store } from '@ngxs/store';
import { SelectBook } from '../books.actions';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { BookDetailsComponent } from '../book-details/book-details.component';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { LibraryAppRoles } from '../app.component';

@Component({
  selector: 'app-book-item',
  templateUrl: './book-item.component.html',
  styleUrls: ['./book-item.component.css'],
  standalone: true,
  imports: [
    MatCardModule,
    MatInputModule, 
    MatButtonModule,
    MatIconModule,
    CommonModule,
    MatDialogModule
  ]
})
export class BookItemComponent {
  neededRoleForEditBook: string = LibraryAppRoles.manager

  constructor(
    private readonly store: Store,
    private readonly api_client: Client,
    public dialog: MatDialog,
    public oidcSecurityService: OidcSecurityService,) {
  }

  openDetails(){
    this.dialog.open(BookDetailsComponent, {
      data: {
        bookId: this.book.id
      }
    })
  }
  
  setBookToEdit(){
      // this.bookService.setBook(this.book)
      this.api_client
      this.store.dispatch(new SelectBook(this.book))
  }
      
  @Input() book!: GetBookDto
}
