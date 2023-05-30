import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditBookComponent } from "../edit-book/edit-book.component";
import { BooksListComponent } from "../books-list/books-list.component";
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { LibraryAppRoles } from '../app.component'

@Component({
    selector: 'app-feature-books',
    standalone: true,
    template: `
    <p class="mat-headline-2">Library</p>
    <app-edit-book *ngIf="(this.oidcSecurityService.getUserData() | async).role == neededRoleForEditBook"></app-edit-book>
    <app-books-list></app-books-list>
  `,
    styles: [],
    imports: [CommonModule, EditBookComponent, BooksListComponent]
})
export class FeatureBooksComponent {
  neededRoleForEditBook: string = LibraryAppRoles.manager

  constructor(public oidcSecurityService: OidcSecurityService) {
    
  }
}
