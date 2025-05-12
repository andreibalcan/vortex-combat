import { Component, input, output } from '@angular/core';
import { DrawerModule } from 'primeng/drawer';
import { AvatarModule } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';

@Component({
	selector: 'app-drawer',
	imports: [DrawerModule, AvatarModule, ButtonModule],
	templateUrl: './drawer.component.html',
	styleUrl: './drawer.component.scss',
})
export class DrawerComponent {
	readonly visible = input(false);
	readonly visibleChange = output<boolean>();

	public close(): void {
		this.visibleChange.emit(false);
	}
}
