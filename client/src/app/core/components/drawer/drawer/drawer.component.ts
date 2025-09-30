import { Component, OnInit, inject } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { DrawerItemComponent } from '../drawer-item/drawer-item.component';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../shared/services/auth/auth.service';

@Component({
	selector: 'app-drawer',
	imports: [CommonModule, DrawerItemComponent],
	templateUrl: './drawer.component.html',
	styleUrl: './drawer.component.scss',
})
export class DrawerComponent implements OnInit {
	private readonly authService: AuthService = inject(AuthService);

	public model: MenuItem[] = [];

	ngOnInit() {
		if (this.authService.hasRole('PrimaryMaster')) {
			this.model = [
				{
					items: [
						{
							label: 'Home',
							icon: 'pi pi-fw pi-home',
							routerLink: ['/home'],
						},
					],
				},
				{
					label: 'Attendance',
					items: [
						{
							label: 'Register Attendance',
							icon: 'pi pi-fw pi-check-circle',
							routerLink: ['/attendance'],
						},
					],
				},
				{
					label: 'Schedule',
					items: [
						{
							label: 'Schedule Workout',
							icon: 'pi pi-fw pi-calendar-plus',
							routerLink: ['/schedule-workout'],
						},
					],
				},
				{
					label: 'Exercises',
					items: [
						{
							label: 'Exercises List',
							icon: 'pi pi-fw pi-list',
							routerLink: ['/exercises'],
						},
						{
							label: 'New Exercise',
							icon: 'pi pi-fw pi-plus',
							routerLink: ['/new-exercise'],
						},
					],
				},
				{
					label: 'Progress',
					items: [
						{
							label: 'Student Progress',
							icon: 'pi pi-fw pi-chart-line',
							routerLink: ['/progress'],
						},
					],
				},
			];
		}

		if (this.authService.hasRole('Student')) {
			this.model = [
				{
					items: [
						{
							label: 'Home',
							icon: 'pi pi-fw pi-home',
							routerLink: ['/home'],
						},
					],
				},
				{
					label: 'Workouts',
					items: [
						{
							label: 'Enroll',
							icon: 'pi pi-fw pi-calendar-plus',
							routerLink: ['/workout-enroll'],
						},
					],
				},
				{
					label: 'Exercises',
					items: [
						{
							label: 'Exercises List',
							icon: 'pi pi-fw pi-list',
							routerLink: ['/exercises'],
						},
					],
				},
				{
					label: 'Progress',
					items: [
						{
							label: 'Student Progress',
							icon: 'pi pi-fw pi-chart-line',
							routerLink: ['/progress'],
						},
					],
				},
			];
		}
	}
}
