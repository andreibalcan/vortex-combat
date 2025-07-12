import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class WorkoutService {
	private readonly http: HttpClient = inject(HttpClient);
	private apiUrl = 'http://localhost:5299/api/workouts';
	private nomisApiUrl = 'http://localhost:5299/nomis/workouts';

	getWorkouts() {
		return this.http.get<any>(`${this.apiUrl}`);
	}

	// TODO: Create DTO
	scheduleWorkout(workout: any) {
		return this.http.post<any>(
			`${this.nomisApiUrl}/schedule-workout`,
			workout
		);
	}

	// TODO: Create DTO
	updateWorkout(workout: any) {
		return this.http.put(
			`${this.nomisApiUrl}/update-workout/${workout.id}`,
			workout
		);
	}

	// TODO: Create DTO
	enrollWorkout(workoutId: string) {
		return this.http.post(
			`${this.nomisApiUrl}/enroll-workout`, { WorkoutId: workoutId }
		);
	}

	deleteWorkout(workoutId: string) {
		return this.http.delete(
			`${this.nomisApiUrl}/delete-workout/${workoutId}`,
		);
	}
}
