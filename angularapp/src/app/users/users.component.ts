import { Component, OnInit, ViewChild } from '@angular/core';
import {MatAccordion, MatExpansionModule} from '@angular/material/expansion';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatNativeDateModule} from '@angular/material/core';
import { LibraryAppRoles } from '../app.component';
import { UserDTO, UsersService } from './users.service';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserService } from 'angular-auth-oidc-client/lib/user-data/user.service';
import {FormsModule} from '@angular/forms';
import {MatSelectModule} from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [  
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatExpansionModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    RouterModule
  ],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  roles = Object.values(LibraryAppRoles)
  users$: Observable<UserViewModel[]> = of()

  @ViewChild(MatAccordion)
  accordion!: MatAccordion;

  constructor(public usersService: UsersService, private router: Router, private activatedRoute: ActivatedRoute){
  }

  ngOnInit(): void {
    console.log('on init')
    this.users$ = this.usersService.getUsers()
    .pipe(
      map(x=> x.map(y => <UserViewModel>{
        ...y,
        hidePassword: true
      }))
    )
  }

  onSubmitEdit(user: UserViewModel){
    this.usersService.updateUser(user).subscribe(x=> {
      console.log(`Edited with result : ${x}`)
    })
    alert('You saved the user!')
  }

  onSubmitRemove(user: UserViewModel){
    this.usersService.removeUser(user.username).subscribe(x=> {
      console.log(`Removed with result : ${x}`)
    })

    window.location.reload()
  }


}

interface UserViewModel{
  username: string,
  password: string,
  role: LibraryAppRoles,
  hidePassword: boolean
}
