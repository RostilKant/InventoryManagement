import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {UserForEditingModel} from "../shared/models/user/userForEditing.model";
import {Observable} from "rxjs";
import {host} from "../shared/models/constants";
import {ChangePasswordModel} from "../shared/models/user/changePassword.model";

@Injectable()
export class UserService {

  constructor(private http: HttpClient) { }

  getUser(): Observable<UserForEditingModel>{
    return this.http.get<UserForEditingModel>(`${host}/users`);
  }

  updateUser(userForEditing: UserForEditingModel): Observable<void>{
    return this.http.post<void>(`${host}/users/update`, userForEditing);
  }

  changeUserPassword(changePasswordModel: ChangePasswordModel): Observable<void>{
    return this.http.post<void>(`${host}/users/change-pass`, changePasswordModel);
  }
}
