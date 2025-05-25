import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { BadgeModule } from 'primeng/badge';
import { ChipModule } from 'primeng/chip';
import { WorkoutService } from '../../shared/services/workout/workout.service';
import { Subscription } from 'rxjs';

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
export class AttendanceComponent implements OnInit, OnDestroy {
	private readonly httpClient: HttpClient = inject(HttpClient);
	private readonly workoutService: WorkoutService = inject(WorkoutService);
	private getWorkoutsSubscription: Subscription = new Subscription();

	// TODO: Add DTOs
	public workouts = [];
	public selectedWorkout: any;

	public students = [];
	public selectedStudents = [];

	public masters = [];
	public selectedMasters = [];

	ngOnInit() {
		this.getWorkoutsSubscription = this.workoutService
			.getWorkouts()
			.subscribe(workouts => {
				this.workouts = workouts.map((workout: any) => ({
					...workout,
					start: this.utcToLocalString(workout.startDate),
					end: this.utcToLocalString(workout.endDate),
				}));
			});

		// TODO: Create services for these endpoints.
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

	private utcToLocalString(utcDateString: string): string {
		const date = new Date(utcDateString);
		const offset = date.getTimezoneOffset();
		const adjustedDate = new Date(date.getTime() - offset * 60000);
		return adjustedDate.toISOString().slice(0, 16).replace('T', ' ');
	}

	ngOnDestroy(): void {
		if (this.getWorkoutsSubscription) {
			this.getWorkoutsSubscription.unsubscribe();
		}
	}
}
