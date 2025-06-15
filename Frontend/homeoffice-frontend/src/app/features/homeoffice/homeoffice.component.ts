import { Component, inject, signal, OnDestroy, computed } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { TimeTrackingService } from '../../core/services/time-tracking.service';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-homeoffice.component',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  providers: [DatePipe],
  templateUrl: './homeoffice.component.html',
  styleUrl: './homeoffice.component.scss'
})
export class HomeofficeComponent implements OnDestroy {
  authService = inject(AuthService);
  timeTrackingService = inject(TimeTrackingService);
  router = inject(Router);
  datePipe = inject(DatePipe);

  currentUser = this.authService.currentUser();
  isTracking = computed(() => this.timeTrackingService.isTracking());
  elapsedTime = signal('00:00:00');
  private timerSubscription?: Subscription;

  description = signal<string | null>(null);
  isLoading = signal(false);
  errorMessage = signal('');

  constructor() {
    if (this.isTracking()) {
      this.startTimer();
    }
  }

  ngOnDestroy() {
    this.stopTimer();
  }

  private startTimer() {
    const startTime = this.timeTrackingService.activeEntry()?.startTime;
    if (!startTime) {
      return;
    }

    this.timerSubscription = interval(1000).subscribe(() => {
      const now = new Date();
      const start = new Date(startTime);
      const diff = now.getTime() - start.getTime();

      const hours = Math.floor(diff / (1000 * 60 * 60));
      const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
      const seconds = Math.floor((diff % (1000 * 60)) / 1000);

      this.elapsedTime.set(
        `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`
      );
    });
  }

  private stopTimer() {
    if (this.timerSubscription) {
      this.timerSubscription.unsubscribe();
      this.timerSubscription = undefined;
    }
  }

  startTracking(): void {
    if (this.isLoading()) {
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.timeTrackingService.start(this.description()).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.startTimer();
      },
      error: (err) => {
        this.errorMessage.set(err.error?.message || 'Ein Fehler ist aufgetreten.');
        this.isLoading.set(false);
      }
    });
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
        this.stopTimer();
        this.elapsedTime.set('00:00:00');
      },
      error: (err) => {
        this.errorMessage.set(err.error?.message || 'Ein Fehler ist aufgetreten.');
        this.isLoading.set(false);
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
