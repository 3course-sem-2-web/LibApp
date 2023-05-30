import {Action, Selector, State, StateContext} from '@ngxs/store';
import { SaveANewBookDto, GetBookDto } from './api.service';
import { Injectable } from '@angular/core';

export class SelectBook {
    static readonly type = '[Book] Select'
    constructor(public hello: SaveANewBookDto) {
        
    }
}

export class ClearBook {
    static readonly type = '[Book] Clear'
    constructor(public emptyTemplate: SaveANewBookDto) {
    }
}

export class AddBookToList {
    static readonly type = '[Book] Add'
    constructor(public bookToAdd: GetBookDto) {
    }
}

export class LoadBooksList {
    static readonly type = '[Book] Load'
    constructor(public booksFromApi: GetBookDto[]) {
    }
}

export interface SelectedBookModel{
    selectedBook: SaveANewBookDto
}

export interface BooksListModel{
    booksList: GetBookDto[]
}

@Injectable()
@State<BooksListModel>({
    name: 'bookslist'
})
export class BooksListState {
    constructor() {
    }
    
    @Selector()
    static getBooksList(state: BooksListModel): GetBookDto[]{
        return state.booksList
    }
    
    @Action(AddBookToList)
    saveBook({patchState, getState}: StateContext<BooksListModel>, {bookToAdd}: AddBookToList): void{
        patchState({
            booksList: [...getState().booksList, {
                ...bookToAdd,
                rating: 0,
                reviewsNumber: 0
            }]
        })
    }

    @Action(LoadBooksList)
    loadBooks({patchState, getState}: StateContext<BooksListModel>, {booksFromApi}: LoadBooksList){
        console.log('getting all books');
        patchState({
            booksList: [...booksFromApi]
        }) 
    }
}

@Injectable()
@State<SelectedBookModel>({
    name: 'selectedbook',
})
export class SelectedBookState {
    constructor() {
    }

    @Selector()
    static getSelectedBook(state: SelectedBookModel): SaveANewBookDto{
        console.log(state.selectedBook);
        
        return state.selectedBook
    }

    @Action(SelectBook)
    selectBook({patchState, getState}: StateContext<SelectedBookModel>, {hello}: SelectBook): void
    {
        patchState({selectedBook: hello})
    }

    @Action(ClearBook)
    clearBook({patchState, getState}: StateContext<SelectedBookModel>, {emptyTemplate}: ClearBook){
        patchState({selectedBook: {
            ...emptyTemplate
        }})
    }
}

