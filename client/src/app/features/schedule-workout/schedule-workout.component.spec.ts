import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleWorkoutComponent } from './schedule-workout.component';

describe('ScheduleWorkoutComponent', () => {
	let component: ScheduleWorkoutComponent;
	let fixture: ComponentFixture<ScheduleWorkoutComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [ScheduleWorkoutComponent],
		}).compileComponents();

		fixture = TestBed.createComponent(ScheduleWorkoutComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
