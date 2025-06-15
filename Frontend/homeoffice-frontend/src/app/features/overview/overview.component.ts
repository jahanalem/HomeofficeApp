import { CommonModule, DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { IHomeOfficeEntry } from 'src/app/core/models/home-office.model';
import { TimeTrackingService } from 'src/app/core/services/time-tracking.service';

@Component({
  selector: 'app-overview.component',
  standalone: true,
  imports: [CommonModule, FormsModule],
  providers: [DatePipe],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.scss'
})
export class OverviewComponent {
  timeTrackingService = inject(TimeTrackingService);
  router = inject(Router);
  datePipe = inject(DatePipe);

  selectedDate = signal<string>(this.getTodayAsString());
  entries = signal<IHomeOfficeEntry[]>([]);
  isLoading = signal(false);
  errorMessage = signal('');

  constructor() {
    this.fetchEntries();
  }

  fetchEntries(): void {
    if (this.isLoading()) {
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');
    this.entries.set([]);

    const date = new Date(this.selectedDate());
    const startDate = new Date(date.setHours(0, 0, 0, 0));
    const endDate = new Date(date.setHours(23, 59, 59, 999));

    this.timeTrackingService.getOverview(startDate, endDate).subscribe({
      next: (data) => {
        this.entries.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set(err.error?.message || 'Einträge konnten nicht geladen werden.');
        this.isLoading.set(false);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/homeoffice']);
  }

  private getTodayAsString(): string {
    return this.datePipe.transform(new Date(), 'yyyy-MM-dd') || '';
  }
}
