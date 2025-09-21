import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewExerciseComponent } from './new-exercise.component';

describe('NewExerciseComponent', () => {
  let component: NewExerciseComponent;
  let fixture: ComponentFixture<NewExerciseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewExerciseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewExerciseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
