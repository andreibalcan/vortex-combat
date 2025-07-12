import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
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
	],
	templateUrl: './edit-workout-modal.component.html',
	styleUrl: './edit-workout-modal.component.scss',
})
export class EditWorkoutModalComponent {
	private readonly ref = inject(DynamicDialogRef);
	private readonly formBuilder: FormBuilder = inject(FormBuilder);
	public readonly config = inject(DynamicDialogConfig);
	
	public workoutForm: FormGroup;
	private readonly initialFormValue: any;

	constructor() {
		this.workoutForm = this.formBuilder.group({
			title: [this.config.data.title, Validators.required],
			start: [new Date(this.config.data.start), Validators.required],
			end: [new Date(this.config.data.end), Validators.required],
			location: [this.config.data.location, Validators.required],
		});

		this.initialFormValue = this.workoutForm.getRawValue();
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
}
