import { CommonModule } from '@angular/common';
import { Component, inject, ViewEncapsulation } from '@angular/core';
import {
	AbstractControl,
	FormBuilder,
	FormGroup,
	FormsModule,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { PanelModule } from 'primeng/panel';
import { Subscription } from 'rxjs';
import { SelectOption } from '../../../shared/types/select';
import { ExerciseService } from '../../../shared/services/exercise/exercise.service';
import { DividerModule } from 'primeng/divider';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { SelectModule } from 'primeng/select';
import { ButtonModule } from 'primeng/button';
import { ToastService } from '../../../core/services/toast.service';
import { belts, degrees } from '../../../shared/constants/belts';
import { categories } from '../../../shared/constants/workout-categories';
import { difficulties } from '../../../shared/constants/difficulties';

@Component({
	selector: 'app-new-exercise',
	imports: [
		CommonModule,
		PanelModule,
		DividerModule,
		FormsModule,
		ReactiveFormsModule,
		InputGroupModule,
		InputGroupAddonModule,
		InputTextModule,
		InputNumberModule,
		SelectModule,
		ButtonModule,
	],
	templateUrl: './new-exercise.component.html',
	styleUrl: './new-exercise.component.scss',
	encapsulation: ViewEncapsulation.None,
})
export class NewExerciseComponent {
	private readonly formBuilder: FormBuilder = inject(FormBuilder);
	private newExerciseSubscription: Subscription = new Subscription();
	private readonly exerciseService: ExerciseService = inject(ExerciseService);
	private readonly toastService: ToastService = inject(ToastService);

	public readonly belts: SelectOption[] = belts;
	public readonly degrees: SelectOption[] = degrees;
	public readonly categories: SelectOption[] = categories;
	public readonly difficulties: SelectOption[] = difficulties;

	public newExerciseForm: FormGroup = this.formBuilder.group({
		name: ['', Validators.required],
		description: ['', Validators.required],
		category: ['', Validators.required],
		difficulty: ['', Validators.required],
		grade: this.formBuilder.group({
			color: ['', Validators.required],
			degrees: ['', Validators.required],
		}),
		beltLevelMin: this.formBuilder.group({
			color: ['', Validators.required],
			degrees: ['', Validators.required],
		}),
		beltLevelMax: this.formBuilder.group({
			color: ['', Validators.required],
			degrees: ['', Validators.required],
		}),
		duration: ['', Validators.required],
		minYearsOfTraining: [null, Validators.required],
		videoURL: ['', Validators.required],
	});

	private markAllControlsDirty(formGroup: FormGroup): void {
		Object.values(formGroup.controls).forEach(
			(control: AbstractControl) => {
				if (control instanceof FormGroup) {
					this.markAllControlsDirty(control);
				} else {
					control.markAsDirty();
				}
			}
		);
	}

	public onSubmit(): void {
		if (this.newExerciseForm.invalid) {
			this.markAllControlsDirty(this.newExerciseForm);
			return;
		}

		this.newExerciseSubscription = this.exerciseService
			.addExercise([this.newExerciseForm.value])
			.subscribe({
				next: res => {
					this.toastService.success(
						'Success!',
						'New exercise created successfully.'
					);
					this.newExerciseForm.reset({
						name: '',
						description: '',
						category: '',
						difficulty: '',
						grade: { color: '', degrees: '' },
						beltLevelMin: { color: '', degrees: '' },
						beltLevelMax: { color: '', degrees: '' },
						duration: '',
						minYearsOfTraining: null,
						videoURL: '',
					});
				},
				error: err => {
					this.toastService.error(
						'Error!',
						'New exercise creation failed:',
						err
					);
				},
			});
	}

	ngOnDestroy(): void {
		if (this.newExerciseSubscription) {
			this.newExerciseSubscription.unsubscribe();
		}
	}
}
