import { Component, inject, OnDestroy } from '@angular/core';
import { CalendarComponent } from '@schedule-x/angular';
import {
	CalendarApp,
	createCalendar,
	createViewWeek,
} from '@schedule-x/calendar';
import { createCurrentTimePlugin } from '@schedule-x/current-time';
import { createDragAndDropPlugin } from '@schedule-x/drag-and-drop';
import { createEventsServicePlugin } from '@schedule-x/events-service';
import { createResizePlugin } from '@schedule-x/resize';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ThemeService } from '../../core/services/theme.service';
import { ToastService } from '../../core/services/toast.service';
import { ScheduleWorkoutModalComponent } from '../../shared/components/modals/schedule-workout-modal/schedule-workout-modal.component';
import { WorkoutService } from '../../shared/services/workout/workout.service';
import { Subscription } from 'rxjs';
import { EditWorkoutModalComponent } from '../../shared/components/modals/edit-workout-modal/edit-workout-modal.component';

@Component({
	selector: 'app-schedule-workout',
	imports: [CalendarComponent],
	templateUrl: './schedule-workout.component.html',
	styleUrl: './schedule-workout.component.scss',
	providers: [DialogService],
})
export class ScheduleWorkoutComponent implements OnDestroy {
	private eventsServicePlugin = createEventsServicePlugin();
	private ref: DynamicDialogRef | undefined;
	private readonly workoutService: WorkoutService = inject(WorkoutService);
	private readonly dialogService: DialogService = inject(DialogService);
	private readonly toastService: ToastService = inject(ToastService);
	private readonly themeService: ThemeService = inject(ThemeService);
	private getWorkoutsSubscription: Subscription = new Subscription();
	private updateWorkoutSubscription: Subscription = new Subscription();
	private closeDateTimeModalSubscription: Subscription = new Subscription();
	private closeEditWorkoutModalSubscription: Subscription = new Subscription();
	private scheduleWorkoutSubscription: Subscription = new Subscription();
	private deleteWorkoutSubscription: Subscription = new Subscription();
	private themeSubscription: Subscription = new Subscription();

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
		plugins: [
			this.eventsServicePlugin,
			createCurrentTimePlugin(),
			createResizePlugin(30),
			createDragAndDropPlugin(30),
		],
		callbacks: {
			onEventUpdate: updatedEvent => {
				const updatedWorkout = {
					id: updatedEvent.id,
					description: updatedEvent.title,
					room: updatedEvent.location,
					startDate: new Date(updatedEvent.start).toISOString(),
					endDate: new Date(updatedEvent.end).toISOString(),
				};

				this.updateWorkoutSubscription = this.workoutService
					.updateWorkout(updatedWorkout)
					.subscribe({
						next: () => {
							this.toastService.success(
								'Workout updated successfully'
							);
						},
						error: () => {
							this.toastService.error('Failed to update workout');
						},
					});
			},
			onBeforeEventUpdate(oldEvent, newEvent) {
				const hasChanged =
					oldEvent.title !== newEvent.title ||
					oldEvent.location !== newEvent.location ||
					new Date(oldEvent.start).getTime() !==
						new Date(newEvent.start).getTime() ||
					new Date(oldEvent.end).getTime() !==
						new Date(newEvent.end).getTime();

				return hasChanged; // If false, update is cancelled
			},
			onClickDateTime: data => this.handleDateTimeClick(data),
			onEventClick: data => this.handleEventClick(data),
		},
	});

	private handleEventClick(data: any) {
		this.ref = this.dialogService.open(EditWorkoutModalComponent, {
			data: data,
			header: data.title,
			modal: true,
			closable: true,
			width: '500px',
			styleClass: 'auto-height-schedule-dialog',
		});

		this.closeEditWorkoutModalSubscription = this.ref.onClose.subscribe(
			closeData => {
				if (closeData.workout) {
					const updatedWorkout = {
						id: data.id,
						description: closeData.workout.title,
						room: closeData.workout.location,
						startDate: new Date(
							closeData.workout.start
						).toISOString(),
						endDate: new Date(closeData.workout.end).toISOString(),
					};

					this.updateWorkoutSubscription = this.workoutService
						.updateWorkout(updatedWorkout)
						.subscribe({
							next: response => {
								this.eventsServicePlugin.update({
									id: data.id,
									location: updatedWorkout.room,
									start: this.utcToLocalString(
										updatedWorkout.startDate
									),
									end: this.utcToLocalString(
										updatedWorkout.endDate
									),
									title: updatedWorkout.description,
								});
								this.toastService.success(
									'Workout updated successfully'
								);
							},
							error: () => {
								this.toastService.error(
									'Failed to update workout'
								);
							},
						});
				}

				if (closeData.delete) {
					this.deleteWorkoutSubscription = this.workoutService
						.deleteWorkout(data.id)
						.subscribe({
							next: response => {
								this.eventsServicePlugin.remove(data.id);
								this.toastService.success(
									'Workout deleted successfully'
								);
							},
							error: () => {
								this.toastService.error(
									'Failed to delete workout'
								);
							},
						});
				}
			}
		);
	}

	private handleDateTimeClick(data: string) {
		this.ref = this.dialogService.open(ScheduleWorkoutModalComponent, {
			data: {
				start: new Date(new Date(data).setMinutes(0, 0, 0)),
				end: new Date(
					new Date(new Date(data).setMinutes(0, 0, 0)).getTime() +
						60 * 60 * 1000
				),
			},
			header: 'Schedule a workout',
			modal: true,
			closable: true,
			width: '500px',
			styleClass: 'auto-height-schedule-dialog',
		});

		// TODO: Add workout DTO
		this.closeDateTimeModalSubscription = this.ref.onClose.subscribe(
			(workout: any) => {
				if (!workout) {
					return;
				}

				const formattedWorkout = {
					description: workout.title,
					room: workout.location,
					startDate: workout.start,
					endDate: workout.end,
				};

				this.scheduleWorkoutSubscription = this.workoutService
					.scheduleWorkout(formattedWorkout)
					.subscribe({
						next: response => {
							this.eventsServicePlugin.add({
								id: response.id,
								title: response.description,
								start: this.utcToLocalString(
									response.startDate
								),
								end: this.utcToLocalString(response.endDate),
								location: response.room,
							});

							this.toastService.success(
								'Workout scheduled successfully'
							);
						},
						error: () => {
							this.toastService.error(
								'Failed to schedule workout'
							);
						},
					});
			}
		);
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
		if (this.updateWorkoutSubscription) {
			this.updateWorkoutSubscription.unsubscribe();
		}
		if (this.closeDateTimeModalSubscription) {
			this.closeDateTimeModalSubscription.unsubscribe();
		}
		if (this.closeEditWorkoutModalSubscription) {
			this.closeEditWorkoutModalSubscription.unsubscribe();
		}
		if (this.deleteWorkoutSubscription) {
			this.deleteWorkoutSubscription.unsubscribe();
		}
		if (this.scheduleWorkoutSubscription) {
			this.scheduleWorkoutSubscription.unsubscribe();
		}
		if (this.themeSubscription) {
			this.themeSubscription.unsubscribe();
		}
	}
}
