import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { EventTypes, OidcSecurityService, PublicEventsService } from 'angular-auth-oidc-client';
import {MatIconModule} from '@angular/material/icon';
import {MatToolbarModule} from '@angular/material/toolbar';
import { AuthService } from './auth/auth.service';
import { distinctUntilChanged, filter } from 'rxjs';

export enum LibraryAppRoles{ admin= "Admin", manager = "Manager", user = "User"}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  neededRoleForEditBook: string = LibraryAppRoles.admin

  isAuthenticated: boolean = false;
  currentUsername?: string
  currentRole?: LibraryAppRoles 

  constructor(
    public oidcSecurityService: OidcSecurityService, 
    public authService: AuthService, public eventService: PublicEventsService) {
  }

  ngOnInit(): void {
    // this.authService.checkAuth().subscribe(x=> console.log(`check auth ${JSON.stringify(x)}`))
    //this.authService.token.subscribe(x=> console.log(`token ${x}`));
    // this.oidcSecurityService.isAuthenticated$.subscribe(x=> {
    //   this.isAuthenticated = x.isAuthenticated
    // })
    // this.oidcSecurityService.checkAuth().subscribe(
    //   x=> {
    //     console.log(x);
    //   });
    // this.oidcSecurityService.stsCallback$.subscribe(
    //   x=> {
    //     console.log(`sts call back ${x}`);
        
    //   }
    // )

    // this.oidcSecurityService
    // .checkSessionChanged$
    // .subscribe(x=> {
    //   console.log(`session changed? : ${x}}]`)
    // } );

    // this.oidcSecurityService.checkAuth()

    this.oidcSecurityService
    .checkAuth()
    .pipe(distinctUntilChanged())
    .subscribe(({ isAuthenticated, userData}) => {
      console.log(isAuthenticated, userData);
      this.isAuthenticated = isAuthenticated
      this.currentRole = userData?.role
      this.currentUsername = userData?.sub
    });


    this.eventService.registerForEvents()
    .pipe(filter(x=> x.type == EventTypes.CheckSessionReceived)).subscribe(x=> {
      console.log(`Session received!!! ${x}`);
    })

    this.eventService.registerForEvents().pipe(filter(x=> x.type == EventTypes.UserDataChanged)).subscribe(x=> {
      console.log(`Changed userdata!!!`);
    })

    this.eventService.registerForEvents().pipe(filter(x=> x.type == EventTypes.CheckingAuthFinished)).subscribe(x=> {
      console.log(`Auth finished`);
    })

    this.eventService.registerForEvents().pipe(filter(x=> x.type == EventTypes.CheckingAuthFinishedWithError)).subscribe(x=> {
      console.log(`Error auth!!!`);
    })

    this.eventService.registerForEvents().pipe(filter(x=> x.type == EventTypes.NewAuthenticationResult))
    .subscribe(x=> {
      console.log(`Auth result ${x}!!!`);
    })

  }

  title = 'Library App';

  login() {
    this.oidcSecurityService.authorize()
  }

  refreshSession() {
    this.oidcSecurityService.forceRefreshSession().subscribe((result) => console.log(result));
  }

  logout() {
    this.oidcSecurityService.logoff().subscribe((result) => {
      console.log(result)
      this.isAuthenticated = !this.isAuthenticated
    });
  }

  logoffAndRevokeTokens() {
    this.oidcSecurityService.logoffAndRevokeTokens().subscribe((result) => console.log(`logoff ${result}`));
  }

  revokeRefreshToken() {
    this.oidcSecurityService.revokeRefreshToken().subscribe((result) => console.log(result));
  }

  revokeAccessToken() {
    this.oidcSecurityService.revokeAccessToken().subscribe((result) => console.log(result));
  }
}