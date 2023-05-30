import { NgModule } from '@angular/core';
import { AuthModule, LogLevel } from 'angular-auth-oidc-client';


@NgModule({
    imports: [AuthModule.forRoot({
        config: {
              authority: 'https://localhost:7040',
              redirectUrl: window.location.origin,
              postLogoutRedirectUri: window.location.origin,
              clientId: 'libraryapp.angular.web',
              scope: 'openid offline_access library.api', // 'openid profile offline_access ' + your scopes
              responseType: 'code',
              useRefreshToken: true,
              forbiddenRoute: window.location.origin + '/forbidden',
              renewTimeBeforeTokenExpiresInSeconds: 30,
              secureRoutes: ['https://localhost:7036/', 'https://localhost:7040/'],
            logLevel: LogLevel.Debug,
          }
      })],
    exports: [AuthModule],
})
export class AuthConfigModule {}
