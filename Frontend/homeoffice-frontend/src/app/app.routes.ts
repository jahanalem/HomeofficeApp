import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/auth').then(c => c.Auth)
  },
  {
    path: 'homeoffice',
    loadComponent: () =>
      import('./features/homeoffice/homeoffice').then(c => c.Homeoffice),
    canActivate: [authGuard]
  },
  {
    path: 'overview',
    loadComponent: () =>
      import('./features/overview/overview.component').then(c => c.OverviewComponent),
    canActivate: [authGuard]
  },
  {
    path: '',
    redirectTo: 'homeoffice',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'homeoffice'
  }
];
