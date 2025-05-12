import { Component, signal } from '@angular/core';
import { DrawerComponent } from '../drawer/drawer/drawer.component';
import { HeaderComponent } from '../header/header.component';
import { RouterModule } from '@angular/router';

@Component({
	selector: 'app-layout',
	imports: [DrawerComponent, HeaderComponent, DrawerComponent, RouterModule],
	templateUrl: './layout.component.html',
	styleUrl: './layout.component.scss',
})
export class LayoutComponent {
	public drawerVisible = signal(true);

	public toggleDrawer(): void {
		this.drawerVisible.update(v => !v);
	}
}
