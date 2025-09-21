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

	public readonly belts: SelectOption[] = [
		{
			label: 'White',
			value: 0,
			imagePath: 'assets/images/belts/white.svg',
		},
		{ label: 'Grey', value: 1, imagePath: 'assets/images/belts/grey.svg' },
		{
			label: 'Yellow',
			value: 2,
			imagePath: 'assets/images/belts/yellow.svg',
		},
		{
			label: 'Orange',
			value: 3,
			imagePath: 'assets/images/belts/orange.svg',
		},
		{
			label: 'Green',
			value: 4,
			imagePath: 'assets/images/belts/green.svg',
		},
		{ label: 'Blue', value: 5, imagePath: 'assets/images/belts/blue.svg' },
		{
			label: 'Purple',
			value: 6,
			imagePath: 'assets/images/belts/purple.svg',
		},
		{
			label: 'Brown',
			value: 7,
			imagePath: 'assets/images/belts/brown.svg',
		},
		{
			label: 'Black',
			value: 8,
			imagePath: 'assets/images/belts/black.svg',
		},
		{ label: 'Red', value: 9, imagePath: 'assets/images/belts/red.svg' },
	];

	public readonly degrees: SelectOption[] = [
		{
			label: '0',
			value: 0,
			imagePath: 'assets/images/belts/0-degrees.svg',
		},
		{ label: '1', value: 1, imagePath: 'assets/images/belts/1-degree.svg' },
		{
			label: '2',
			value: 2,
			imagePath: 'assets/images/belts/2-degrees.svg',
		},
		{
			label: '3',
			value: 3,
			imagePath: 'assets/images/belts/3-degrees.svg',
		},
		{
			label: '4',
			value: 4,
			imagePath: 'assets/images/belts/4-degrees.svg',
		},
	];

	public readonly categories: SelectOption[] = [
		{ label: 'Movement', value: 'movement' },
		{ label: 'Takedown', value: 'takedown' },
		{ label: 'Sweep', value: 'sweep' },
		{ label: 'Submission', value: 'submission' },
		{ label: 'Guard', value: 'guard' },
		{ label: 'Escape', value: 'escape' },
		{ label: 'Guard Pass', value: 'guard pass' },
		{ label: 'Takedown Defense', value: 'takedown defense' },
		{ label: 'Control', value: 'control' },
		{ label: 'Transition', value: 'transition' },
		{ label: 'Positional', value: 'positional' },
		{ label: 'Teaching', value: 'teaching' },
		{ label: 'Philosophy', value: 'philosophy' },
		{ label: 'Conditioning', value: 'conditioning' },
	];

	public readonly difficulties: SelectOption[] = [
		{ label: 'Very Easy', value: 1 },
		{ label: 'Easy', value: 2 },
		{ label: 'Hard', value: 3 },
		{ label: 'Very Hard', value: 4 },
		{ label: 'Expert', value: 4 },
	];

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
