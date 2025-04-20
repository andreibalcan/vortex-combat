import { Component } from '@angular/core';
import { Router } from '@angular/router';
import {
	FormBuilder,
	FormGroup,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';

@Component({
	selector: 'app-login',
	standalone: true,
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss'],
	imports: [CommonModule, ReactiveFormsModule],
})
export class LoginComponent {
	loginForm: FormGroup;
	errorMessage: string = '';

	constructor(
		private authService: AuthService,
		private router: Router,
		private fb: FormBuilder
	) {
		this.loginForm = this.fb.group({
			email: ['', [Validators.required, Validators.email]],
			password: ['', [Validators.required]],
		});
	}

	onSubmit() {
		if (this.loginForm.invalid) {
			return;
		}

		const { email, password } = this.loginForm.value;
		this.authService.login(email, password).subscribe({
			next: (res) => {
				this.authService.setToken(res.token); // Make sure your API returns { token: string }
				this.router.navigate(['/attendance']);
			},
			error: (err) => {
				console.error('Login failed:', err);
			},
		});
	}
}
