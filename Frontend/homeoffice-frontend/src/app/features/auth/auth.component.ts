import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { Router } from '@angular/router';
import { ILoginRequest } from '../../core/models/auth.model';

@Component({
  selector: 'app-auth.component',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss'
})
export class AuthComponent {
  authService = inject(AuthService);
  router = inject(Router);

  username = signal<string>('');
  password = signal<string>('');

  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  login(): void {
    if (this.isLoading()) {
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    const credentials: ILoginRequest = {
      username: this.username(),
      password: this.password()
    };

    this.authService.login(credentials).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set(err.error?.message || 'Login fehlgeschlagen. Bitte versuchen Sie es erneut.');
        this.isLoading.set(false);
      }
    });
  }
}
