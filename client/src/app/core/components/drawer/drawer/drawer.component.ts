import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { DrawerItemComponent } from '../drawer-item/drawer-item.component';
import { CommonModule } from '@angular/common';
@Component({
	selector: 'app-drawer',
	imports: [CommonModule, DrawerItemComponent],
	templateUrl: './drawer.component.html',
	styleUrl: './drawer.component.scss',
})
export class DrawerComponent implements OnInit {
	public model: MenuItem[] = [];

	ngOnInit() {
		this.model = [
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
		];
	}
}
