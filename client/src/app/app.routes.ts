import { Routes } from '@angular/router';
import { AttendanceComponent } from './attendance/attendance.component';
import { LoginComponent } from './login/login.component';
import { authGuard } from './guards/auth.guard';
import { roleGuard } from './guards/role.guard';

export const routes: Routes = [
	{ path: '', redirectTo: '/login', pathMatch: 'full' }, // TODO: Confirm to which page to redirect based on login status
	{ path: 'login', component: LoginComponent },
	{
		path: 'attendance',
		component: AttendanceComponent,
		canActivate: [authGuard, roleGuard(['PrimaryMaster'])],
	},
];
