import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Injectable({ providedIn: 'root' })
export class AuthService {
	private apiUrl = 'http://localhost:5299/api/auth';
	private readonly ROLE_CLAIM =
		'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

	constructor(private http: HttpClient, private router: Router) {}

	login(email: string, password: string): Observable<any> {
		return this.http.post<any>(`${this.apiUrl}/login`, { email, password });
	}

	setToken(token: string) {
		localStorage.setItem('jwt', token);
	}

	getToken(): string | null {
		return localStorage.getItem('jwt');
	}

	isAuthenticated(): boolean {
		const token = this.getToken();
		if (!token) return false;

		try {
			const decodedToken: any = jwtDecode(token);
			const isExpired = decodedToken.exp < Date.now() / 10000;
			return !isExpired;
		} catch (error) {
			return false;
		}
	}

	getUserRoles(): string[] {
		const token = this.getToken();
		if (!token) return [];

		try {
			const decodedToken: any = jwtDecode(token);
			if (decodedToken[this.ROLE_CLAIM]) {
				return Array.isArray(decodedToken[this.ROLE_CLAIM])
					? decodedToken[this.ROLE_CLAIM]
					: [decodedToken[this.ROLE_CLAIM]];
			}
			return [];
		} catch (error) {
			return [];
		}
	}

	hasRole(role: string): boolean {
		const roles = this.getUserRoles();
		return roles.includes(role);
	}

	logout() {
		localStorage.removeItem('jwt');
		this.router.navigate(['/login']);
	}
}
