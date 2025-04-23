import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AttendanceComponent } from './attendance/attendance.component';
import { authGuard } from './guards/auth.guard';
import { roleGuard } from './guards/role.guard';

export const routes: Routes = [
	{ path: '', redirectTo: '/login', pathMatch: 'full' }, // TODO: Confirm to which page to redirect based on login status
	{ path: 'login', title: 'Login', component: LoginComponent },
    { path: 'register', title: 'Register', component: RegisterComponent},
	{
		path: 'attendance',
        title: 'Register Attendance'
,		component: AttendanceComponent,
		canActivate: [authGuard, roleGuard(['PrimaryMaster'])],

	},
];
