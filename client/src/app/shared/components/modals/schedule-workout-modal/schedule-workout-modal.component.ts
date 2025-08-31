import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import {
	FormBuilder,
	FormGroup,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { DatePickerModule } from 'primeng/datepicker';
import { InputTextModule } from 'primeng/inputtext';
import { MultiSelectModule } from 'primeng/multiselect';
import { ExerciseService } from '../../../services/exercise/exercise.service';
import { Subscription } from 'rxjs';

@Component({
	selector: 'app-schedule-workout-modal',
	imports: [
		CommonModule,
		ReactiveFormsModule,
		InputGroupModule,
		InputGroupAddonModule,
		InputTextModule,
		ButtonModule,
		DatePickerModule,
		MultiSelectModule,
	],
	templateUrl: './schedule-workout-modal.component.html',
	styleUrl: './schedule-workout-modal.component.scss',
})
export class ScheduleWorkoutModalComponent implements OnInit, OnDestroy {
	private readonly ref = inject(DynamicDialogRef);
	private readonly formBuilder: FormBuilder = inject(FormBuilder);
	private readonly config = inject(DynamicDialogConfig);
	private readonly exerciseService: ExerciseService = inject(ExerciseService);

	private exercisesSubscription: Subscription = new Subscription();
	public exercises = [];

	ngOnInit(): void {
		this.getExercises();
	}

	public scheduleForm: FormGroup = this.formBuilder.group({
		title: ['New workout', Validators.required],
		start: [this.config.data.start, Validators.required],
		end: [this.config.data.end, Validators.required],
		location: ['', Validators.required],
		exercises: ['', Validators.required],
	});

	private getExercises(): void {
		this.exercisesSubscription = this.exerciseService
			.getExercises()
			.subscribe(exercises => (this.exercises = exercises));
	}

	public submit(): void {
		if (this.scheduleForm.valid) {
			this.ref.close(this.scheduleForm.value);
		}
	}

	public cancel(): void {
		this.ref.close();
	}

	ngOnDestroy(): void {
		if (this.exercisesSubscription) {
			this.exercisesSubscription.unsubscribe();
		}
	}
}
