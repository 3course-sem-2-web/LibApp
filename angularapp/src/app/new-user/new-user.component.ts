import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LibraryAppRoles } from '../app.component';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { UserService } from 'angular-auth-oidc-client/lib/user-data/user.service';
import { UserDTO, UsersService } from '../users/users.service';
import { Route, Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-new-user',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule, 
    MatInputModule, 
    MatIconModule, 
    ReactiveFormsModule,
    MatSelectModule,
    MatButtonModule,
    RouterModule
  ],
  templateUrl: './new-user.component.html',
  styleUrls: ['./new-user.component.css']
})
export class NewUserComponent {
  roles = Object.values(LibraryAppRoles)

  newUserForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    role: new FormControl(LibraryAppRoles.manager),
  });

  constructor(private usersService: UsersService, private router: Router) {
  }

  onSumbit(){
    this.usersService
      .createUser({ username: this.newUserForm.value.username!, role: this.newUserForm.value.role! })
      .subscribe(x=> {
        console.log(`Create user responded ${x}`);
        alert('User successfully created')

        this.router.navigate(['/users'])
      })
  }
}
