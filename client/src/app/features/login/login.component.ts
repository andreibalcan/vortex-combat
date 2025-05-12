import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
	FormBuilder,
	FormGroup,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { CheckboxModule } from 'primeng/checkbox';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { AuthService } from '../../shared/services/auth/auth.service';
import { ToastService } from '../../core/services/toast.service';

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
	private readonly toastService: ToastService = inject(ToastService);
	private readonly authService: AuthService = inject(AuthService);
	private readonly router: Router = inject(Router);

	public readonly loginForm: FormGroup;

	constructor() {
		this.loginForm = this.formBuilder.group({
			email: ['', [Validators.required, Validators.email]],
			password: ['', [Validators.required]],
		});
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
		this.toastService.success(
			'Authentication Successful!',
			'You have logged in successfully.'
		);
	}

	private loginError(err: any): void {
		this.toastService.error(
			'Authentication failed!',
			'Invalid credentials, please try again.'
		);
	}
}
