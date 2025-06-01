import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { DatePickerModule } from 'primeng/datepicker';
import { InputTextModule } from 'primeng/inputtext';
import { AccordionModule } from 'primeng/accordion';
import { BadgeModule } from 'primeng/badge';

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
export class WorkoutEnrollModalComponent {
	private readonly ref = inject(DynamicDialogRef);
	private readonly formBuilder: FormBuilder = inject(FormBuilder);
	public readonly config = inject(DynamicDialogConfig);

	public workoutForm: FormGroup = this.formBuilder.group({
		title: { value: this.config.data.title, disabled: true },
		start: { value: new Date(this.config.data.start), disabled: true },
		end: { value: new Date(this.config.data.end), disabled: true },
		location: { value: this.config.data.location, disabled: true },
	});

	submit(): void {
		this.ref.close({ enrolled: true });
	}

	// TODO: Create endpoint and implement the undo enroll / remove enroll action.
	remove(): void {
		this.ref.close({ enrolled: false });
	}

	cancel(): void {
		this.ref.close();
	}
}
