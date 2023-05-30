import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpTokenInterceptorService implements HttpInterceptor {
  constructor(private authService: AuthService,
              private oidcSecurityService: OidcSecurityService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Send token only for API requests
        if (request.url.includes('/api'))
        {
        request = request.clone(
            {
            setHeaders: {
                'Authorization': 'Bearer ' + this.oidcSecurityService.getIdToken(),
                "Access-Control-Allow-Origin": "localhost:4200"
            }
            });
        }
        
        return next.handle(request);
    }
}