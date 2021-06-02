import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from "@angular/router";
import {Observable} from "rxjs";
import {AuthService} from "./auth.service";

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private auth: AuthService, private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot)
    : Observable<boolean> | Promise<boolean> | boolean {
    const token = this.auth.token

    if (token == 'expires'){
      this.auth.logout()
      this.router.navigate(['/auth', 'login'], {
        queryParams: {
          sessionExpired: true
        }
      })
      return false
    }
    else if (token == null) {
      this.auth.logout()
      this.router.navigate(['/auth', 'login'], {
        queryParams: {
          pleaseLogin: true
        }
      })
      return false
    }
    else
      return true
  }

}
