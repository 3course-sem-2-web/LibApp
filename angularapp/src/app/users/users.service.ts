import { Injectable } from '@angular/core';
import { LibraryAppRoles } from '../app.component';
import { Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private readonly usersBaseURL = 'https://localhost:7040'

  FAKEUSERS: UserDTO[]= [
    { username: 'User1', role: LibraryAppRoles.admin},
    { username: 'User2', role: LibraryAppRoles.manager},
    { username: 'User3', role: LibraryAppRoles.user},
  ]

  users : Observable<UserDTO[]> = of(this.FAKEUSERS)

  constructor(private httpClient: HttpClient) {
  }

  createUser(user: UserDTO): Observable<UserDTO>{
    return this.httpClient.post<UserDTO>(this.usersBaseURL + `/user/${user.username}`, user)
  }

  updateUser(user: UserDTO){
    return this.httpClient.put(this.usersBaseURL + `/user/${user.username}`, user)
  }
  removeUser(username: string){
    
    return this.httpClient.delete(this.usersBaseURL + `/user/${username}`)
  }
  getUsers(): Observable<UserDTO[]>{
    return this.httpClient.get<UserDTO[]>(this.usersBaseURL + `/users`)
  }
}

export interface UserDTO{
  username: string,
  role: LibraryAppRoles
}