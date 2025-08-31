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
import { AccordionModule } from 'primeng/accordion';
import { BadgeModule } from 'primeng/badge';
import { Subscription } from 'rxjs';
import { ExerciseService } from '../../../services/exercise/exercise.service';
import { MultiSelectModule } from 'primeng/multiselect';

@Component({
	selector: 'app-edit-workout-modal',
	imports: [
		CommonModule,
		ReactiveFormsModule,
		InputGroupModule,
		InputGroupAddonModule,
		InputTextModule,
		ButtonModule,
		DatePickerModule,
		AccordionModule,
		BadgeModule,
		MultiSelectModule,
	],
	templateUrl: './edit-workout-modal.component.html',
	styleUrl: './edit-workout-modal.component.scss',
})
export class EditWorkoutModalComponent implements OnInit, OnDestroy {
	private readonly ref = inject(DynamicDialogRef);
	private readonly formBuilder: FormBuilder = inject(FormBuilder);
	public readonly config = inject(DynamicDialogConfig);

	public workoutForm: FormGroup;
	private readonly initialFormValue: any;

	private readonly exerciseService: ExerciseService = inject(ExerciseService);
	private exercisesSubscription: Subscription = new Subscription();
	public exercises = [];

	constructor() {
		this.workoutForm = this.formBuilder.group({
			title: [this.config.data.title, Validators.required],
			start: [new Date(this.config.data.start), Validators.required],
			end: [new Date(this.config.data.end), Validators.required],
			location: [this.config.data.location, Validators.required],
			exercises: [this.config.data.exercises, Validators.required],
		});

		this.initialFormValue = this.workoutForm.getRawValue();
	}

	ngOnInit(): void {
		this.getExercises();
	}

	private getExercises(): void {
		this.exercisesSubscription = this.exerciseService
			.getExercises()
			.subscribe(exercises => (this.exercises = exercises));
	}

	public submit(): void {
		this.ref.close({ workout: this.workoutForm.value, delete: false });
	}

	// TODO: Create endpoint and implement the undo enroll / remove enroll action.
	public remove(): void {
		this.ref.close({ workout: null, delete: true });
	}

	public cancel(): void {
		this.ref.close({ workout: null, delete: false });
	}

	public hasFormChanged(): boolean {
		return (
			JSON.stringify(this.initialFormValue) !==
			JSON.stringify(this.workoutForm.getRawValue())
		);
	}

	ngOnDestroy(): void {
		if (this.exercisesSubscription) {
			this.exercisesSubscription.unsubscribe();
		}
	}
}
