import {Injectable} from '@angular/core';
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {Router} from '@angular/router';
import {catchError, tap} from 'rxjs/operators';
import {AuthService} from "./auth.service";


@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private auth: AuthService,
  ) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.auth.isAuthenticated()) {
      req = req.clone({
        setHeaders: {
          'Authorization': this.auth.token as string,
          Accept: '*/*'
        }
      })
    }
    return next.handle(req)
      .pipe(
        tap(() => {
          console.log('Intercept')
        })/*,
        catchError((error: HttpErrorResponse) => {
          console.log(error);
          if (error.status === 401) {
            this.auth.logout();
            this.router.navigate(['/admin', 'login'], {
              queryParams: {
                authFailed: true
              }
            });
          }
          return throwError(error);
        })*/
      )
  }

}
