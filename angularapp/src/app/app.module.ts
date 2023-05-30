import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { AuthConfigModule } from './auth/auth-config.module';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { AuthInterceptor, AuthModule } from 'angular-auth-oidc-client';
import { NgxsModule } from '@ngxs/store';
import { BooksListState, SelectedBookState } from './books.actions';
import { EditBookComponent } from "./edit-book/edit-book.component";
import { BooksListComponent } from "./books-list/books-list.component";
import { API_BASE_URL } from './api.service';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import {MatTooltipModule} from '@angular/material/tooltip';
import { BooksPageComponent } from "./books-page/books-page.component";
import { BookItemComponent } from './book-item/book-item.component';
import { BookDetailsComponent } from './book-details/book-details.component';
import { FeatureBooksComponent } from './feature-books/feature-books.component';
import {MatMenuModule} from '@angular/material/menu'
import { CommonModule } from '@angular/common';
import { NgForm } from '@angular/forms';

@NgModule({
    declarations: [
        AppComponent,
        ForbiddenComponent,
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
        { provide: API_BASE_URL, useValue: 'https://localhost:7036' },
    ],
    bootstrap: [AppComponent],
    imports: [
        BrowserModule, AuthConfigModule, AppRoutingModule, BrowserAnimationsModule,
        MatIconModule, MatToolbarModule, MatButtonModule, MatTooltipModule, MatMenuModule,
        HttpClientModule,
        NgxsModule.forRoot([SelectedBookState, BooksListState], {
            developmentMode: false
        }),
        BooksListComponent,
        EditBookComponent,
        BooksPageComponent,
        BookItemComponent,
        BookDetailsComponent,
        CommonModule,
    ]
})
export class AppModule { }
