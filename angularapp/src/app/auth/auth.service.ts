import { Injectable } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable, map, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(public oidcSecurityService: OidcSecurityService) { }

  get isLoggedIn() {
    return this.oidcSecurityService.isAuthenticated$;
  }

  get accesstoken() {
    return this.oidcSecurityService.getAccessToken()
  }

  get idtoken(){
    return this.oidcSecurityService.getIdToken()
  }

  get userData() {
    return this.oidcSecurityService.userData$;
  }

  checkAuth() {
    return this.oidcSecurityService.checkAuth()
  }

  doLogin() {
    return of(this.oidcSecurityService.authorize())
  }

  signOut() {
    this.oidcSecurityService.logoff()
  }
}
