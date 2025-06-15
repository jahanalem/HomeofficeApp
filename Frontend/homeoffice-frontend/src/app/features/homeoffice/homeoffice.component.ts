import { Component, computed, inject, signal } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { TimeTrackingService } from '../../core/services/time-tracking.service';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-homeoffice.component',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './homeoffice.component.html',
  styleUrl: './homeoffice.component.scss'
})
export class HomeofficeComponent {
  authService = inject(AuthService);
  timeTrackingService = inject(TimeTrackingService);
  router = inject(Router);

  currentUser = this.authService.currentUser();
  isTracking = computed(() => this.timeTrackingService.isTracking());

  description = signal<string | null>(null);

  isLoading = signal(false);
  errorMessage = signal('');


  startTracking(): void {
    if (this.isLoading()) {
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.timeTrackingService.start(this.description()).subscribe({
      next: () => {
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set(err.error?.message || 'Ein Fehler ist aufgetreten.');
        this.isLoading.set(false);
      }
    }
    )
  }

  stopTracking(): void {
    if (this.isLoading()) {
      return;
    }
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.timeTrackingService.stop().subscribe({
      next: () => {
        this.description.set('');
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set(err.error?.message || 'Ein Fehler ist aufgetreten.');
        this.isLoading.set(false);
      }
    })
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
