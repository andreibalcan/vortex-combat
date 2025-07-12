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
import { MasterService } from '../../shared/services/users/master.service';
import { StudentService } from '../../shared/services/users/student.service';
import { ToastService } from '../../core/services/toast.service';

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
	private readonly masterService: MasterService = inject(MasterService);
	private readonly studentService: StudentService = inject(StudentService);
	private readonly toastService: ToastService = inject(ToastService);
	private getWorkoutsSubscription: Subscription = new Subscription();
	private getMastersSubscription: Subscription = new Subscription();
	private getStudentsSubscription: Subscription = new Subscription();
	private registerAttendanceSubscription: Subscription = new Subscription();

	// TODO: Add DTOs
	public workouts: any = [];
	public selectedWorkout: any;

	public students: any = [];
	public selectedStudents = [];

	public masters: any = [];
	public selectedMasters = [];

	ngOnInit() {
		this.getWorkouts();
		this.getMasters();
		this.getStudents();
	}

	private getWorkouts(): void {
		this.getWorkoutsSubscription = this.workoutService
			.getWorkouts()
			.subscribe(workouts => {
				this.workouts = workouts.map((workout: any) => ({
					...workout,
					start: this.utcToLocalString(workout.startDate),
					end: this.utcToLocalString(workout.endDate),
				}));
			});
	}

	private getStudents(): void {
		this.getStudentsSubscription = this.studentService
			.getStudents()
			.subscribe(students => {
				this.students = students;
			});
	}

	private getMasters(): void {
		this.getMastersSubscription = this.masterService
			.getMasters()
			.subscribe(masters => {
				this.masters = masters;
			});
	}

	public submitAttendance(): void {
		let attendanceList = {
			workoutId: this.selectedWorkout.id,
			studentIds: this.selectedStudents.map((el: any) => el.id),
			masterIds: this.selectedMasters.map((el: any) => el.id),
		};

		this.registerAttendanceSubscription = this.workoutService
			.registerAttendance(attendanceList)
			.subscribe({
				next: response => {
					this.toastService.success(
						'Attendance registered successfully'
					);
				},
				error: () => {
					this.toastService.error('Failed to register attendance');
				},
			});
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
		if (this.getMastersSubscription) {
			this.getMastersSubscription.unsubscribe();
		}
		if (this.getStudentsSubscription) {
			this.getStudentsSubscription.unsubscribe();
		}
		if (this.registerAttendanceSubscription) {
			this.registerAttendanceSubscription.unsubscribe();
		}
	}
}
