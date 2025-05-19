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

@Component({
	selector: 'app-schedule-workout-modal',
	imports: [
		CommonModule,
		ReactiveFormsModule,
		InputGroupModule,
		InputGroupAddonModule,
		InputTextModule,
		ButtonModule,
		DatePickerModule
	],
	templateUrl: './schedule-workout-modal.component.html',
	styleUrl: './schedule-workout-modal.component.scss',
})
export class ScheduleWorkoutModalComponent {
	private readonly ref = inject(DynamicDialogRef);
	private readonly formBuilder: FormBuilder = inject(FormBuilder);
	private readonly config = inject(DynamicDialogConfig);
	
	public scheduleForm: FormGroup = this.formBuilder.group({
		title: ['New workout', Validators.required],
		start: [this.config.data.start, Validators.required],
		end: [this.config.data.end, Validators.required],
		location: ['', Validators.required],
		people: '',
	});

	submit(): void {
		if (this.scheduleForm.valid) {
			this.ref.close(this.scheduleForm.value);
		}
	}

	cancel(): void {
		this.ref.close();
	}
}
