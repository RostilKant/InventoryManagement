import {Injectable} from "@angular/core";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {UserForAuthenticationModel} from "../models/user/userForAuthentication.model";
import {host} from "../models/constants";
import {Observable, Subject, throwError} from "rxjs";
import {catchError, tap} from "rxjs/operators";
import {TokenModel} from "../models/token.model";

@Injectable()
export class AuthService {

  public error$: Subject<string> = new Subject<string>();

  constructor(private http: HttpClient) {

  }

  get token(): string | null {
    const expiresDate: Date = new Date(localStorage.getItem('jwt-token-exp') as string)

    if (new Date() > expiresDate){
      this.logout()
      return null
    }
    return localStorage.getItem('jwt-token') as string
  }

  login(user: UserForAuthenticationModel): Observable<TokenModel> {
    return this.http.post<TokenModel>(`${host}/users/login`, user)
      .pipe(
        tap(AuthService.setToken),
        catchError(this.handleError.bind(this))
      );
  }

  logout() {
    localStorage.clear()
  }

  isAuthenticated(): boolean {
    return !!this.token;
  }

  private static setToken(response: TokenModel) {
      localStorage.setItem('jwt-token', `Bearer ${response.token}`)
      localStorage.setItem('jwt-token-exp', `${response.exp}`)
  }

  private handleError(error: HttpErrorResponse) {

    if (error.status == 401 || error.statusText == 'Unauthorized'){
      this.error$.next('Wrong Email or Password');
    }
    return throwError(error)
  }
}
