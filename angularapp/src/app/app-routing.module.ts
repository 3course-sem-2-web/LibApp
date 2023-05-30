import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { AutoLoginPartialRoutesGuard } from 'angular-auth-oidc-client';
import { HomeComponent } from './home/home.component';
import { FeatureBooksComponent } from './feature-books/feature-books.component';
import { AdminGuardService } from './auth/admin-guard.service';
import { UsersComponent } from './users/users.component';
import { NewUserComponent } from './new-user/new-user.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'users', component: UsersComponent,canActivate: [AutoLoginPartialRoutesGuard, AdminGuardService] },
  { path: 'books', component: FeatureBooksComponent, canActivate: [AutoLoginPartialRoutesGuard] },
  { path: 'newuser', component: NewUserComponent },
  { path: 'unauthorized', component: UnauthorizedComponent , canActivate: [AutoLoginPartialRoutesGuard]},
  { path: 'forbidden', component: ForbiddenComponent },
  { path: '**', redirectTo: 'home', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation:'reload' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }