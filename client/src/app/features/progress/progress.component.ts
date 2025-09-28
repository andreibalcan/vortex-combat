import {
	Component,
	inject,
	OnDestroy,
	OnInit,
	signal,
	ViewEncapsulation,
} from '@angular/core';
import { ProgressService } from '../../shared/services/progress/progress.service';
import type { Subscription } from 'rxjs';
import type { ChartData, ChartOptions } from 'chart.js';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { AvatarModule } from 'primeng/avatar';
import { ChartModule } from 'primeng/chart';
import { BadgeModule } from 'primeng/badge';
import { ProgressBarModule } from 'primeng/progressbar';
import { StepsModule } from 'primeng/steps';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { PanelModule } from 'primeng/panel';
import { SelectOption } from '../../shared/types/select';
import { ChipModule } from 'primeng/chip';
import { AuthService } from '../../shared/services/auth/auth.service';
import { StudentService } from '../../shared/services/users/student.service';
import { InputGroupModule } from 'primeng/inputgroup';
import { SelectModule } from 'primeng/select';
import { InputGroupAddon } from 'primeng/inputgroupaddon';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { belts, degrees } from '../../shared/constants/belts';

@Component({
	selector: 'app-progress',
	standalone: true,
	imports: [
		CommonModule,
		FormsModule,
		CardModule,
		InputGroupModule,
		InputGroupAddon,
		SelectModule,
		ChartModule,
		BadgeModule,
		ProgressBarModule,
		TableModule,
		ButtonModule,
		PanelModule,
		ChipModule,
		RouterLink,
	],
	templateUrl: './progress.component.html',
	styleUrls: ['./progress.component.scss'],
	encapsulation: ViewEncapsulation.None,
})
export class ProgressComponent implements OnInit, OnDestroy {
	public authService = inject(AuthService);
	private progressService = inject(ProgressService);
	private studentService = inject(StudentService);
	private progressSubscription: Subscription | null = null;
	private studentSubscription: Subscription | null = null;
	public progress = signal<any | null>(null);
	public students = signal<any | null>(null);

	public selectedStudentId: number | null = null;

	onStudentChange(studentId: number): void {
		if (studentId) {
			this.getStudentProgress(studentId);
		}
	}

	
	ngOnInit(): void {
		if (this.authService.hasRole('PrimaryMaster')) {
			this.getStudents();
		}
		if (this.authService.hasRole('Student')) {
			this.getStudentProgress();
		}
	}

	public getBeltColor(beltColor: number): SelectOption | undefined {
		return belts.find(belt => belt.value === beltColor);
	}

	public getBeltDegrees(beltDegrees: number): SelectOption | undefined {
		return degrees.find(degree => degree.value === beltDegrees);
	}

	private getStudents(): void {
		this.studentSubscription = this.studentService
			.getStudents()
			.subscribe(students => this.students.set(students));
	}

	private getStudentProgress(studentId?: number): void {
		this.progressSubscription = this.progressService
			.getStudentProgress(studentId)
			.subscribe(progress => {
				this.progress.set(progress);
				// Update chart data
				const completedCount = progress.completedExercises?.length ?? 0;
				const remainingCount = progress.remainingExercises?.length ?? 0;
				this.exerciseChartData.datasets[0].data = [
					completedCount,
					remainingCount,
				];

				this.exerciseChartData.labels = [
					`Completed - ${completedCount}`,
					`Remaining - ${remainingCount}`,
				];
			});
	}

	public exerciseChartData: ChartData<'doughnut'> = {
		labels: ['Completed', 'Remaining'],
		datasets: [
			{
				data: [0, 0],
				backgroundColor: ['#10b981', '#f97316'],
				hoverBackgroundColor: ['#10b981', '#f97316'],
			},
		],
	};

	public exerciseChartOptions: ChartOptions<'doughnut'> = {
		responsive: true,
		plugins: {
			legend: {
				position: 'bottom',
				labels: {
					padding: 20,
				},
			},
			tooltip: {
				callbacks: {
					label: tooltipItem => {
						const value = tooltipItem.raw as number;
						return `${tooltipItem.label}: ${value}`;
					},
				},
			},
		},
	};

	ngOnDestroy(): void {
		if (this.progressSubscription) {
			this.progressSubscription.unsubscribe();
		}
		if (this.studentSubscription) {
			this.studentSubscription.unsubscribe();
		}
	}
}
