import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkoutEnrollComponent } from './workout-enroll.component';

describe('WorkoutEnrollComponent', () => {
  let component: WorkoutEnrollComponent;
  let fixture: ComponentFixture<WorkoutEnrollComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkoutEnrollComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkoutEnrollComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
