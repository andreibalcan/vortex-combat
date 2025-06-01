import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkoutEnrollModalComponent } from './workout-enroll-modal.component';

describe('WorkoutEnrollModalComponent', () => {
  let component: WorkoutEnrollModalComponent;
  let fixture: ComponentFixture<WorkoutEnrollModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkoutEnrollModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkoutEnrollModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
