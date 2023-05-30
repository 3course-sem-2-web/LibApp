import { Component, OnInit } from '@angular/core';
import {MatCardModule} from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { SaveANewBookDto, Client } from '../api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { Select, Store } from '@ngxs/store';
import { SelectedBookState, ClearBook, AddBookToList } from '../books.actions';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { AuthService } from '../auth/auth.service';


enum SaveModeTitles{
  Add = "Add",
  Update = "Update"
}

@Component({
  selector: 'app-edit-book',
  templateUrl: './edit-book.component.html',
  styleUrls: ['./edit-book.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatDividerModule,
    MatGridListModule,
    MatInputModule,
    MatIconModule,
    FormsModule
  ]
})
export class EditBookComponent implements OnInit {
  
  public coverFilename: string ='';

  saveModeTitle: string = SaveModeTitles.Add
  
  constructor(
    private api_client: Client,
    private readonly store: Store,
    public authService: AuthService
  ){}

  ngOnInit(): void {
    this.resetForm()
    
    this.selectedBook$?.subscribe(x=>
      {
        this.form = x
      }
    )
  }

  @Select(SelectedBookState.getSelectedBook) 
  selectedBook$? : Observable<SaveANewBookDto>

  public form!: SaveANewBookDto;
  
  handleFileInputChange(files: FileList){
    const coverImage = files[0]
    
    if(coverImage == undefined){
      console.log('no file given');
      return;
    }

    this.convertToBase64(coverImage)
  }

  convertToBase64(file: File){
    console.log(file.name);
    
    this.coverFilename = file.name
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      const intermediateResultWithIncomingFiletype = reader.result!.toString();
      // const base64String = intermediateResultWithIncomingFiletype.split(',')[1]
      const base64String = intermediateResultWithIncomingFiletype
      this.form.cover = base64String;
    };
    reader.onerror = (error) => {
      console.log('Error:', error);
    };
  }

  save(){
    console.log('handling add request');
  
    this.api_client
    .save(this.form)
    .subscribe((x)=>{
      if (this.form.id === 0) {
        const addedBookIdAsUntypedObj: string = JSON.stringify(x)
        const addedBookIdAsTypedObj: {Id:number} = JSON.parse(addedBookIdAsUntypedObj)
        this.form.id = addedBookIdAsTypedObj.Id
        this.store.dispatch([new AddBookToList(this.form)])
      }
      this.resetForm()
    });

  }

  resetForm(){
    this.store.dispatch(new ClearBook(EditBookComponent.getBookTemplate()))
    this.coverFilename = ''
  }

  public static getBookTemplate(): SaveANewBookDto{
    return {
      id: 0, 
      title: '', 
      author : '', 
      content : '', 
      cover: '', 
      genre: '',
    }
  }

}
