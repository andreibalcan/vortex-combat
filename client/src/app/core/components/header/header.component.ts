import { Component, EventEmitter, inject, Output } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { MenubarModule } from 'primeng/menubar';
import { AuthService } from '../../../shared/services/auth/auth.service';

@Component({
	selector: 'app-header',
	imports: [MenubarModule, ButtonModule],
	templateUrl: './header.component.html',
	styleUrl: './header.component.scss',
})
export class HeaderComponent {
	private readonly authService: AuthService = inject(AuthService);

	public readonly items: MenuItem[] = [
		{
			label: 'item1',
			icon: 'arrow',
		},
	];
	@Output() toggleDrawer = new EventEmitter<void>();

	public onMenuClick(): void {
		this.toggleDrawer.emit();
	}

	public onLogoutClick(): void {
		this.authService.logout();
	}
}
