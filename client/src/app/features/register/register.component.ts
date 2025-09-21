import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy } from '@angular/core';
import {
	FormBuilder,
	FormGroup,
	ReactiveFormsModule,
	Validators,
	AbstractControl,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DatePickerModule } from 'primeng/datepicker';
import { DividerModule } from 'primeng/divider';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { SelectModule } from 'primeng/select';

import { AuthService } from '../../shared/services/auth/auth.service';
import {
	identityPasswordValidator,
	passwordMatchValidator,
} from '../../shared/validators/password.validator';
import { Subscription } from 'rxjs';
import { SelectOption } from '../../shared/types/select';

@Component({
	selector: 'app-register',
	standalone: true,
	imports: [
		CommonModule,
		ReactiveFormsModule,
		CardModule,
		InputGroupModule,
		InputGroupAddonModule,
		DividerModule,
		InputTextModule,
		InputNumberModule,
		SelectModule,
		DatePickerModule,
		PasswordModule,
		ButtonModule,
		RouterLink,
	],
	templateUrl: './register.component.html',
	styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnDestroy {
	private readonly formBuilder: FormBuilder = inject(FormBuilder);
	private readonly authService: AuthService = inject(AuthService);
	private readonly router: Router = inject(Router);
	private registerSubscription: Subscription = new Subscription();

	public readonly genders: SelectOption[] = [
		{ label: 'Male', value: 0 },
		{ label: 'Female', value: 1 },
	];

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

	public readonly minBirthdayDate: Date = new Date(
		new Date().setFullYear(new Date().getFullYear() - 150)
	);

	public readonly maxBirthdayDate: Date = new Date();

	public registerForm: FormGroup = this.formBuilder.group(
		{
			name: ['', Validators.required],
			nif: ['', [Validators.maxLength(9), Validators.required]],
			gender: ['', Validators.required],
			birthday: ['', Validators.required],
			address: this.formBuilder.group({
				street: [''],
				number: [''],
				floor: [''],
				city: [''],
				zip: [''],
			}),
			belt: this.formBuilder.group({
				color: [null, Validators.required],
				degrees: [null, Validators.required],
			}),
			height: [null, Validators.required],
			weight: [null, Validators.required],
			email: ['', [Validators.required, Validators.email]],
			phoneNumber: ['', Validators.required],
			password: ['', [Validators.required, identityPasswordValidator]],
			passwordConfirmation: [
				'',
				[Validators.required, identityPasswordValidator],
			],
		},
		{ validators: passwordMatchValidator }
	);

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
		if (this.registerForm.invalid) {
			this.markAllControlsDirty(this.registerForm);
			return;
		}

		this.registerSubscription = this.authService
			.register(this.registerForm.value)
			.subscribe({
				next: res => {
					this.authService.setToken(res.token);
					this.router.navigate(['/login']);
				},
				error: err => {
					console.error('Register failed:', err);
				},
			});
	}

	ngOnDestroy(): void {
		if (this.registerSubscription) {
			this.registerSubscription.unsubscribe();
		}
	}
}
