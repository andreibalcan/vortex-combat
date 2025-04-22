import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const identityPasswordValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
	const value = control.value || '';

	const errors: ValidationErrors = {};

	if (value.length < 6) {
		errors['minLength'] = true;
	}
	if (!/[A-Z]/.test(value)) {
		errors['uppercase'] = true;
	}
	if (!/[a-z]/.test(value)) {
		errors['lowercase'] = true;
	}
	if (!/[0-9]/.test(value)) {
		errors['digit'] = true;
	}
	if (!/[!@#$%^&*(),.?":{}|<>_\-+=\\[\]]/.test(value)) {
		errors['specialChar'] = true;
	}

	return Object.keys(errors).length > 0 ? errors : null;
};

export const passwordMatchValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
	const password = control.get('password')?.value;
	const confirmPassword = control.get('passwordConfirmation')?.value;

	if (password && confirmPassword && password !== confirmPassword) {
		return { passwordsMismatch: true };
	}

	return null;
};