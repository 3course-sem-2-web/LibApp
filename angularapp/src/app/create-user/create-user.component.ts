import { Component } from '@angular/core';
import { FormControl, FormsModule } from '@angular/forms';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { MatExpansionModule } from '@angular/material/expansion';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-user',
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
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatRippleModule,
    MatButtonModule, 
    MatIconModule, 
    FormsModule,
  ],
  template: `
    <p class="mat-header-2">New user</p>
    <form >
      <div class="example-container">
        <mat-form-field>
          <mat-label>Enter your password</mat-label>
          <input matInput>
        </mat-form-field>

        <mat-form-field floatLabel="always">
          <mat-label>Amount</mat-label>
          <input matInput type="number" class="example-right-align" placeholder="0">
        </mat-form-field>
      </div>

    </form>
  `,
  styles: [
  ]
})
export class CreateUserComponent {
  username = new FormControl(, )
  submit(){

  }
}
