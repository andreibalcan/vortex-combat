import { Component, input, output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AvatarModule } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { DrawerModule } from 'primeng/drawer';

@Component({
	selector: 'app-drawer',
	imports: [DrawerModule, AvatarModule, ButtonModule, RouterLink],
	templateUrl: './drawer.component.html',
	styleUrl: './drawer.component.scss',
})
export class DrawerComponent {
	public readonly visible = input(false);
	public readonly visibleChange = output<boolean>();

	public close(): void {
		this.visibleChange.emit(false);
	}
}
