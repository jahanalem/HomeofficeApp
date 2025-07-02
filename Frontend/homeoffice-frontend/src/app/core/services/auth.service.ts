import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environment';
import { ILoginRequest, ILoginResponse } from '../models/auth.model';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, tap, throwError } from 'rxjs';
import { LOCAL_STORAGE_KEYS } from '../constants/auth';
import { StorageService } from './storage.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl = environment.apiUrl;
  private readonly apiUrl = `${this.baseUrl}/account`;

  currentUser = signal<ILoginResponse | null | undefined>(undefined);
  isLoggedIn = computed(() => !!this.currentUser());

  private readonly http = inject(HttpClient);
  private readonly storageService = inject(StorageService);
  private readonly router = inject(Router);

  constructor() {
    const user = localStorage.getItem(LOCAL_STORAGE_KEYS.AUTH_TOKEN);
    if (user) {
      this.currentUser.set(JSON.parse(user));
    }
    else {
      this.currentUser.set(null);
    }
  }

  login(values: ILoginRequest) {
    return this.http.post<ILoginResponse>(`${this.apiUrl}/login`, values, { withCredentials: true }).pipe(
      tap(response => {
        this.storageService.set(LOCAL_STORAGE_KEYS.AUTH_TOKEN, response);
        this.currentUser.set(response);
      })
    );
  }

  logout() {
    this.http.post(`${this.apiUrl}/logout`, {}, { withCredentials: true }).subscribe();

    this.storageService.delete(LOCAL_STORAGE_KEYS.AUTH_TOKEN);
    this.currentUser.set(null);
    this.router.navigate(['/login']);
  }

  // The refresh token is in an HttpOnly cookie, so we send an empty body.
  // The backend will return the new access token as json object.
  refreshToken(): Observable<string> {
    return this.http.post<{ token: string }>(
      `${this.apiUrl}/refresh-token`,
      {},
      { withCredentials: true }
    ).pipe(
      map(response => response.token), // Extract token from response object
      tap(newToken => {
        const currentUser = this.currentUser();
        if (currentUser) {
          const updatedUser: ILoginResponse = {
            ...currentUser,
            token: newToken
          };
          this.storeUserData(updatedUser);
        }
      }),
      catchError(error => {
        console.error('Refresh token failed:', error);
        this.logout();
        return throwError(() => error);
      })
    );
  }

  private storeUserData(response: ILoginResponse): void {
    this.storageService.set(LOCAL_STORAGE_KEYS.AUTH_TOKEN, response);
    this.currentUser.set(response);
  }
}

