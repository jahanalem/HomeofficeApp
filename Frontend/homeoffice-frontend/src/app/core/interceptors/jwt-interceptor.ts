import { HttpErrorResponse, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { TokenService } from '../services/token.service';
import { catchError, filter, switchMap, take, throwError } from 'rxjs';
import { StorageService } from '../services/storage.service';
import { LOCAL_STORAGE_KEYS } from '../constants/auth';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const tokenService = inject(TokenService);

  // Skip token handling for auth endpoints
  if (req.url.includes('/account/login') ||
    req.url.includes('/account/refresh-token')) {
    return next(req);
  }

  const token = authService.currentUser()?.token;
  if (token) {
    req = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && authService.isLoggedIn()) {
        return handleTokenExpiration(req, next, authService, tokenService);
      }
      return throwError(() => error);
    })
  );
};

function handleTokenExpiration(
  req: HttpRequest<any>,
  next: HttpHandlerFn,
  authService: AuthService,
  tokenService: TokenService
) {
  if (tokenService.isRefreshing()) {
    return tokenService.getRefreshTokenSubject().pipe(
      filter(token => token !== null),
      take(1),
      switchMap(token => {
        req = req.clone({
          setHeaders: { Authorization: `Bearer ${token}` }
        });
        return next(req);
      })
    );
  }

  tokenService.setRefreshing(true);
  tokenService.setNewToken(null);

  return authService.refreshToken().pipe(
    switchMap(newToken => {
      tokenService.setRefreshing(false);
      tokenService.setNewToken(newToken);

      req = req.clone({
        setHeaders: { Authorization: `Bearer ${newToken}` }
      });
      return next(req);
    }),
    catchError(error => {
      tokenService.setRefreshing(false);
      authService.logout();
      return throwError(() => error);
    })
  );
}
