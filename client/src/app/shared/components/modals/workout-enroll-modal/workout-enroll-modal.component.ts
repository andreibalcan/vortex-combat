import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
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

@Component({
	selector: 'app-workout-enroll-modal',
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
	],
	templateUrl: './workout-enroll-modal.component.html',
	styleUrl: './workout-enroll-modal.component.scss',
})
export class WorkoutEnrollModalComponent implements OnInit, OnDestroy {
	private readonly ref = inject(DynamicDialogRef);
	private readonly formBuilder: FormBuilder = inject(FormBuilder);
	public readonly config = inject(DynamicDialogConfig);

	private readonly exerciseService: ExerciseService = inject(ExerciseService);
	private exerciseSubscription: Subscription = new Subscription();
	public workoutExercises: any = [];

	public workoutForm: FormGroup = this.formBuilder.group({
		title: { value: this.config.data.title, disabled: true },
		start: { value: new Date(this.config.data.start), disabled: true },
		end: { value: new Date(this.config.data.end), disabled: true },
		location: { value: this.config.data.location, disabled: true },
	});

	ngOnInit(): void {
		this.getExercises();
	}

	private getExercises(): void {
		this.exerciseService
			.getExercisesById(this.config.data.exercises)
			.subscribe((exercises: any) => (this.workoutExercises = exercises));
	}

	public submit(): void {
		this.ref.close({ enrolled: true });
	}

	// TODO: Create endpoint and implement the undo enroll / remove enroll action.
	public remove(): void {
		this.ref.close({ enrolled: false });
	}

	public cancel(): void {
		this.ref.close();
	}

	ngOnDestroy(): void {
		if (this.exerciseSubscription) {
			this.exerciseSubscription.unsubscribe();
		}
	}
}
