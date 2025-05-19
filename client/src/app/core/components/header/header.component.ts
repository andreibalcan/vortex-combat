import { Component, inject, output } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { MenubarModule } from 'primeng/menubar';
import { AuthService } from '../../../shared/services/auth/auth.service';
import { ThemeService } from '../../services/theme.service';

@Component({
	selector: 'app-header',
	imports: [MenubarModule, ButtonModule],
	templateUrl: './header.component.html',
	styleUrl: './header.component.scss',
})
export class HeaderComponent {
	private readonly authService: AuthService = inject(AuthService);
	public readonly themeService: ThemeService = inject(ThemeService);
	public readonly toggleDrawer = output<void>();
	
	public readonly items: MenuItem[] = [
		{
			label: 'item1',
			icon: 'arrow',
		},
	];

	public onMenuClick(): void {
		this.toggleDrawer.emit();
	}

	public onLogoutClick(): void {
		this.authService.logout();
	}

	public toggleDarkMode(): void {
		this.themeService.toggleTheme();
	}
}
