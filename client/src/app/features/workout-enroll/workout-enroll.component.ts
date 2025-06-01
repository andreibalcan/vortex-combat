import { Component, inject, OnDestroy } from '@angular/core';
import { CalendarComponent } from '@schedule-x/angular';
import {
	CalendarApp,
	createCalendar,
	createViewWeek,
} from '@schedule-x/calendar';
import { createEventsServicePlugin } from '@schedule-x/events-service';
import { ThemeService } from '../../core/services/theme.service';
import { ToastService } from '../../core/services/toast.service';
import { WorkoutService } from '../../shared/services/workout/workout.service';
import { Subscription } from 'rxjs';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { WorkoutEnrollModalComponent } from '../../shared/components/modals/workout-enroll-modal/workout-enroll-modal.component';

@Component({
	selector: 'app-workout-enroll',
	imports: [CalendarComponent],
	templateUrl: './workout-enroll.component.html',
	styleUrl: './workout-enroll.component.scss',
	providers: [DialogService],
})
export class WorkoutEnrollComponent {
	private eventsServicePlugin = createEventsServicePlugin();
	private ref: DynamicDialogRef | undefined;
	private readonly workoutService: WorkoutService = inject(WorkoutService);
	private readonly dialogService: DialogService = inject(DialogService);
	private readonly toastService: ToastService = inject(ToastService);
	private readonly themeService: ThemeService = inject(ThemeService);
	private getWorkoutsSubscription: Subscription = new Subscription();
	private themeSubscription: Subscription = new Subscription();
	private closeModalSubscription: Subscription = new Subscription();
	private enrollWorkoutSubscription: Subscription = new Subscription();

	constructor() {
		this.getWorkoutsSubscription = this.workoutService
			.getWorkouts()
			.subscribe(workouts => {
				const formattedEvents = workouts.map((workout: any) => ({
					id: workout.id.toString(),
					title: workout.description,
					start: this.utcToLocalString(workout.startDate),
					end: this.utcToLocalString(workout.endDate),
					location: workout.room,
					masters: workout.masters,
					students: workout.students,
				}));

				this.eventsServicePlugin.set(formattedEvents);
			});

		this.themeSubscription = this.themeService.theme$.subscribe(() => {
			this.updateCalendarTheme();
		});
	}

	calendarApp: CalendarApp = createCalendar({
		views: [createViewWeek()],
		locale: 'en-GB',
		isDark: this.themeService.isDarkMode(),
		plugins: [this.eventsServicePlugin],
		callbacks: {
			onEventClick: data => this.handleEventClick(data),
		},
	});

	handleEventClick(data: any) {
		console.log('dataaaaaa', data);
		this.ref = this.dialogService.open(WorkoutEnrollModalComponent, {
			data: data,
			header: 'Enroll in Workout',
			modal: true,
			closable: true,
			width: '500px',
			styleClass: 'auto-height-schedule-dialog',
		});

		this.closeModalSubscription = this.ref.onClose.subscribe(closeData => {
			if (closeData.enrolled) {
				console.log('enrolled', closeData);
				this.enrollWorkoutSubscription = this.workoutService
					.enrollWorkout(data.id)
					.subscribe({
						next: response => {
							this.eventsServicePlugin.update({
								...data,
								students: [...(data.students || []), response],
							});
							this.toastService.success(
								'Enrolled in workout successfully'
							);
						},
						error: () => {
							this.toastService.error(
								'Failed to enroll in workout'
							);
						},
					});
			} else {
				console.log('undo enroll');
				// TODO: Undo enroll
			}
		});
	}

	// TODO: Make this a shared reusable function. Reused in 2 components already.
	private updateCalendarTheme() {
		this.calendarApp.setTheme(
			this.themeService.isDarkMode() ? 'dark' : 'light'
		);
	}

	// TODO: Make this a shared reusable function. Reused in 2 components already.
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
		if (this.closeModalSubscription) {
			this.closeModalSubscription.unsubscribe();
		}
		if (this.themeSubscription) {
			this.themeSubscription.unsubscribe();
		}
		if (this.enrollWorkoutSubscription) {
			this.enrollWorkoutSubscription.unsubscribe();
		}
	}
}
