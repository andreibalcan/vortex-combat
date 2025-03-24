import { Routes } from '@angular/router';
import { AttendanceComponent } from './attendance/attendance.component';

export const routes: Routes = [
  { path: '', redirectTo: '/attendance', pathMatch: 'full' }, // TODO: Replace when new maing page is created
  { path: 'attendance', component: AttendanceComponent },
];
