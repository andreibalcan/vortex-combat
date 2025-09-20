import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { TabsModule } from 'primeng/tabs';
import { BadgeModule } from 'primeng/badge';
import { AvatarModule } from 'primeng/avatar';

import { ExerciseService } from '../../shared/services/exercise/exercise.service';
import { Subscription } from 'rxjs';
import { AccordionComponent } from "../../shared/components/accordion/accordion.component";
import { TExercise } from '../../shared/types/exercise';
import { AccordionModule } from 'primeng/accordion';

@Component({
	selector: 'app-exercise',
	imports: [CommonModule, TabsModule, BadgeModule, AvatarModule, AccordionModule, AccordionComponent],
	templateUrl: './exercise.component.html',
	styleUrl: './exercise.component.scss',
})
export class ExerciseComponent implements OnInit, OnDestroy {
	private readonly exerciseService: ExerciseService = inject(ExerciseService);

	private exercisesSubscription: Subscription = new Subscription();
	public exercises: TExercise[] = [];

	ngOnInit(): void {
		this.getExercises();
	}

	private getExercises(): void {
		this.exercisesSubscription = this.exerciseService
			.getExercises()
			.subscribe((exercises: TExercise[]) => (this.exercises = exercises));
	}

	public getBeltExercises(beltColor: number, degrees: number): TExercise[] {
		return this.exercises.filter(el => el.grade.color === beltColor && el.grade.degrees === degrees);
	}

	ngOnDestroy(): void {
		if (this.exercisesSubscription) {
			this.exercisesSubscription.unsubscribe();
		}
	}
}
