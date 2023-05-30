import { Component, Inject, OnInit } from '@angular/core';
import {MatDialogModule, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { Client, BookDetailsDto } from '../api.service';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import {MatDividerModule} from '@angular/material/divider';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-book-details',
  templateUrl: './book-details.component.html',
  styleUrls: [
    './book-details.component.css',
    './book-details.component.sass'
  ],
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule
  ],
})
export class BookDetailsComponent implements OnInit {
  constructor(
    private readonly api_client: Client,
    @Inject(MAT_DIALOG_DATA) public data: {bookId: number}) {}

  ngOnInit(): void {
    this.api_client.booksGET(this.data.bookId).subscribe(x => {
      this.bookDetails = x
    })
  }

  bookDetails?: BookDetailsDto

}
