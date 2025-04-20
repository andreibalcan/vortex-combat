import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { BadgeModule } from 'primeng/badge';
import { ChipModule } from 'primeng/chip';

@Component({
	selector: 'app-attendance',
	imports: [
		TableModule,
		ButtonModule,
		SelectModule,
		BadgeModule,
		ChipModule,
		FormsModule,
		DatePipe,
	],
	templateUrl: './attendance.component.html',
	styleUrl: './attendance.component.scss',
})
export class AttendanceComponent implements OnInit {
	public workouts = [];
	public selectedWorkout: any;

	public students = [];
	public selectedStudents = [];

	public masters = [];
	public selectedMasters = [];

	constructor(private httpClient: HttpClient) {}

	ngOnInit() {
		this.httpClient
			.get('http://localhost:5299/api/workouts')
			.subscribe((workouts: any) => {
				this.workouts = workouts;
			});
		this.httpClient
			.get('http://localhost:5299/api/masters')
			.subscribe((masters: any) => {
				this.masters = masters;
			});
		this.httpClient
			.get('http://localhost:5299/api/students')
			.subscribe((students: any) => {
				this.students = students;
			});
	}

	public submitAttendance(): void {
		let attendanceList = {
			workoutId: this.selectedWorkout.id,
			studentIds: this.selectedStudents.map((el: any) => el.id),
			masterIds: this.selectedMasters.map((el: any) => el.id),
		};

		this.httpClient
			.post(
				'http://localhost:5299/nomis/workouts/register-attendance',
				attendanceList
			)
			.subscribe();
	}
}
