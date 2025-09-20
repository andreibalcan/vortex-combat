import { Component, inject, input, signal } from '@angular/core';
import { AccordionModule } from 'primeng/accordion';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { TExercise } from '../../types/exercise';
import { PanelModule } from 'primeng/panel';
import { KnobModule } from 'primeng/knob';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
	selector: 'app-accordion',
	imports: [
		CommonModule,
		FormsModule,
		AccordionModule,
		AvatarModule,
		BadgeModule,
		PanelModule,
		KnobModule,
	],
	templateUrl: './accordion.component.html',
	styleUrl: './accordion.component.scss',
})
export class AccordionComponent {
	public exercises = input.required<TExercise[]>();
	public title = input.required<string>();
	public image = input.required<string>();
	public value = input.required<string>();
	public activeVideo = signal<number | null>(null);

	private readonly sanitizer: DomSanitizer = inject(DomSanitizer);

	public getSafeUrl(videoUrl: string) {
		return this.sanitizer.bypassSecurityTrustResourceUrl(
			`${videoUrl}?modestbranding=1&rel=0&disablekb=1`
		);
	}

	public onPanelToggle(exerciseId: number, collapsed: boolean): void {
		if (!collapsed) {
			this.activeVideo.set(exerciseId);
		} else {
			this.activeVideo.set(null);
		}
	}
}
