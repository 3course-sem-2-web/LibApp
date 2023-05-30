import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { LibraryAppRoles } from '../app.component';

@Injectable({
  providedIn: 'root'
})
export class AdminGuardService  implements CanActivate {
  constructor(private oidcSecurityService: OidcSecurityService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> {
    return this.oidcSecurityService.userData$.pipe(
      take(1),
      map((x) => {
        
        // allow navigation if authenticated
        if (x.userData.role == LibraryAppRoles.admin) {
          return true;
        }

        // redirect if not authenticated
        return this.router.parseUrl('/forbidden');
      })
    );
  }
}
