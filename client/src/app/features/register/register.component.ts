import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
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

interface SelectOption {
	label: string;
	value: number;
}

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
export class RegisterComponent {
	private readonly formBuilder: FormBuilder = inject(FormBuilder);
	private readonly authService: AuthService = inject(AuthService);
	private readonly router: Router = inject(Router);

	public readonly genders: SelectOption[] = [
		{ label: 'Male', value: 0 },
		{ label: 'Female', value: 1 },
	];

	public readonly belts: SelectOption[] = [
		{ label: 'White', value: 0 },
		{ label: 'Gray', value: 1 },
		{ label: 'Yellow', value: 2 },
		{ label: 'Orange', value: 3 },
		{ label: 'Green', value: 4 },
		{ label: 'Blue', value: 5 },
		{ label: 'Purple', value: 6 },
		{ label: 'Brown', value: 7 },
		{ label: 'Black', value: 8 },
		{ label: 'Red', value: 9 },
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

		this.authService.register(this.registerForm.value).subscribe({
			next: (res) => {
				this.authService.setToken(res.token);
				this.router.navigate(['/login']);
			},
			error: (err) => {
				console.error('Register failed:', err);
			},
		});
	}
}
