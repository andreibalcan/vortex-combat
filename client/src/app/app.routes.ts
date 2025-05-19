import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login.component';
import { RegisterComponent } from './features/register/register.component';
import { AttendanceComponent } from './features/attendance/attendance.component';
import { ScheduleWorkoutComponent } from './features/schedule-workout/schedule-workout.component';
import { authGuard } from './shared/guards/auth.guard';
import { roleGuard } from './shared/guards/role.guard';
import { ErrorPageComponent } from './core/components/error-page/error-page.component';
import { LayoutComponent } from './core/components/layout/layout.component';

export const routes: Routes = [
	// No layout
	{ path: 'login', title: 'Login', component: LoginComponent },
	{ path: 'register', title: 'Register', component: RegisterComponent },

	// Layout
	{
		path: '',
		component: LayoutComponent,
		children: [
			{ path: '', redirectTo: '/login', pathMatch: 'full' }, // TODO: Create a home page and redirect to it.
			{
				path: 'attendance',
				title: 'Register Attendance',
				component: AttendanceComponent,
				canActivate: [authGuard, roleGuard(['PrimaryMaster'])],
			},
			{
				path: 'schedule-workout',
				title: 'Schedule Workout',
				component: ScheduleWorkoutComponent,
				canActivate: [authGuard, roleGuard(['PrimaryMaster'])],
			},
			{
				path: '**',
				title: 'Page not found',
				component: ErrorPageComponent,
			},
		],
	},
];
