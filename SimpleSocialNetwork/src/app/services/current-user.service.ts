import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from 'rxjs';
import {LimitedUserModel, UserModel, UserApiService} from '../../backend_api_client';
import {HttpErrorResponse} from '@angular/common/http';
import {AuthService} from './auth.service';
import {UnionUserModel} from '../models/helper-types';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {


  constructor(private userService: UserApiService,
              private auth: AuthService) {
    this.userSubject = new BehaviorSubject<UnionUserModel | null>(null);
    this.user = this.userSubject.asObservable();
  }

  private userSubject: BehaviorSubject<UnionUserModel | null>;
  public user: Observable<UnionUserModel | null>;

  get currentUser(): UnionUserModel | null {
    return this.userSubject.value;
  }

  get isCurrentSelf(): boolean {
    return this.auth.getCurrentUserValue()?.login === this.currentUser?.login;
  }

  public changeUserTo(userName: string): void {
    const isMyProfile = this.auth.getCurrentUserValue()?.login === userName;
    ((): Observable<UnionUserModel> => {
      if (isMyProfile || this.auth.isAdmin) {
        return this.userService.apiUserGet(userName).pipe(
          map(us => {
            return {
              modelType: 'full',
              about: us.about,
              login: us.login,
              dateBirth: us.dateBirth,
              isAdmin: us.isAdmin,
              isDeleted: us.isDeleted,
            };
          })
        );
      } else {
        return this.userService.apiUserLimitedGet(userName).pipe(
          map(us => {
            return {
              modelType: 'limited',
              about: us.about,
              login: us.login,
              isDeleted: us.isDeleted,
            };
          })
        );
      }
    })().subscribe({
      next: u => {
        this.userSubject.next(u);
      },
      error: e => {
        this.userSubject.error(e);
      }
    });
  }
}
