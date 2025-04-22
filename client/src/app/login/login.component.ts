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
import { CheckboxModule } from 'primeng/checkbox';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { AuthService } from '../auth.service';
import { identityPasswordValidator } from '../validators/password.validator';

@Component({
	selector: 'app-login',
	standalone: true,
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss'],
	imports: [
		CommonModule,
		ReactiveFormsModule,
		CardModule,
		InputGroupModule,
		InputGroupAddonModule,
		InputTextModule,
		PasswordModule,
		CheckboxModule,
		ButtonModule,
		RouterLink,
	],
})
export class LoginComponent {
	private readonly formBuilder: FormBuilder = inject(FormBuilder);

	public readonly loginForm: FormGroup;

	constructor(
		private readonly authService: AuthService,
		private readonly router: Router
	) {
		this.loginForm = this.formBuilder.group({
			email: ['', [Validators.required, Validators.email]],
			password: ['', [Validators.required, identityPasswordValidator]],
		});
	}

	get password(): AbstractControl {
		return this.loginForm.get('password') as AbstractControl;
	}

	public onSubmit(): void {
		if (this.loginForm.invalid) {
			Object.values(this.loginForm.controls).forEach((control) => {
				control.markAsDirty();
			});
			return;
		}

		const { email, password } = this.loginForm.value;
		this.authService.login(email, password).subscribe({
			next: this.loginSuccess.bind(this),
			error: this.loginError.bind(this),
		});
	}

	private loginSuccess(res: any): void {
		this.authService.setToken(res.token);
		this.router.navigate(['/attendance']);
	}

	private loginError(err: any): void {
		console.error('Login failed:', err);
	}
}
