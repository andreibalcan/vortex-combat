import { Component, inject } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { Router } from '@angular/router';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { CardModule } from 'primeng/card';
import {
	FormBuilder,
	FormGroup,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';

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
		ButtonModule,
	],
})
export class LoginComponent {
	private formBuilder = inject(FormBuilder);

	loginForm: FormGroup = this.formBuilder.group({
		email: ['', [Validators.required, Validators.email]],
		password: ['', Validators.required],
	});

	constructor(private authService: AuthService, private router: Router) {}

	onSubmit() {
		if (this.loginForm.invalid) {
			return;
		}

		const { email, password } = this.loginForm.value;
		this.authService.login(email, password).subscribe({
			next: (res) => {
				this.authService.setToken(res.token);
				this.router.navigate(['/attendance']);
			},
			error: (err) => {
				console.error('Login failed:', err);
			},
		});
	}
}
