import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class StudentService {
	private readonly http: HttpClient = inject(HttpClient);
	private apiUrl = 'http://localhost:5299/api';

	// TODO: Create DTO
	getStudents() {
		return this.http.get<any>(`${this.apiUrl}/students`);
	}
}
