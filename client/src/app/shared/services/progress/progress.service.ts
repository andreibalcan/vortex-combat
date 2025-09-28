import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class ProgressService {
	private readonly http: HttpClient = inject(HttpClient);
	private nomisApiUrl = 'http://localhost:5299/nomis/students/progress';

	public getStudentProgress(studentId?: number) {
		const url =
			studentId != null
				? `${this.nomisApiUrl}/${studentId}`
				: `${this.nomisApiUrl}`;

		return this.http.get<any>(url);
	}
}
