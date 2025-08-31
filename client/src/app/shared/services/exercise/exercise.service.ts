import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class ExerciseService {
	private readonly http: HttpClient = inject(HttpClient);
	private nomisApiUrl = 'http://localhost:5299/nomis/exercise';

	public getExercises() {
		return this.http.get<any>(`${this.nomisApiUrl}`);
	}

	public getExercisesById(id?: number[]) {
		let params = new HttpParams();

		if (id && id.length > 0) {
			params = params.set('id', id.join(','));
		}

		return this.http.get<any[]>(`${this.nomisApiUrl}`, { params });
	}
}
