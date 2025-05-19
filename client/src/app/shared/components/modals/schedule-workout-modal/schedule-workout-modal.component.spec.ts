import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleWorkoutModalComponent } from './schedule-workout-modal.component';

describe('ScheduleWorkoutModalComponent', () => {
	let component: ScheduleWorkoutModalComponent;
	let fixture: ComponentFixture<ScheduleWorkoutModalComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [ScheduleWorkoutModalComponent],
		}).compileComponents();

		fixture = TestBed.createComponent(ScheduleWorkoutModalComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
