import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environment';
import { ILoginRequest, ILoginResponse } from '../models/auth.model';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl = environment.apiUrl;
  private readonly apiUrl  = `${this.baseUrl}/account`;

  currentUser = signal<ILoginResponse | null | undefined>(undefined);
  isLoggedIn  = computed(() => !!this.currentUser());

  private readonly http = inject(HttpClient);

  constructor() {
    const user = localStorage.getItem("user_token");
    if (user) {
      this.currentUser.set(JSON.parse(user));
    }
    else {
      this.currentUser.set(null);
    }
  }

  login(values: ILoginRequest) {
    this.http.post<ILoginResponse>(`${this.apiUrl}/login`, values).pipe(
      tap(response => {
        localStorage.setItem('user_token', JSON.stringify(response));
        this.currentUser.set(response);
      })
    );
  }

  logout() {
    localStorage.removeItem('user_token');
    this.currentUser.set(null);
  }
}

