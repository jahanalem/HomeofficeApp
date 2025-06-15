import { IHomeOfficeEntry } from './../models/home-office.model';
import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environment';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TimeTrackingService {
  private readonly apiUrl = `${environment.apiUrl}/timetracking`;

  http = inject(HttpClient);

  readonly activeEntry = signal<IHomeOfficeEntry | null>(null);
  readonly isTracking = computed(() => !!this.activeEntry());

  constructor() { }

  start(description: string | null): Observable<IHomeOfficeEntry> {
    return this.http.post<IHomeOfficeEntry>(`${this.apiUrl}/start`, description).pipe(
      tap(response => this.activeEntry.set(response))
    );
  }

  stop(): Observable<IHomeOfficeEntry> {
    return this.http.post<IHomeOfficeEntry>(`${this.apiUrl}/stop`, {}).pipe(
      tap(() => this.activeEntry.set(null))
    );
  }
}
