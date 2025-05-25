import { Component, inject } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { MenubarModule } from 'primeng/menubar';
import { AuthService } from '../../../shared/services/auth/auth.service';
import { ThemeService } from '../../services/theme.service';
import { CommonModule } from '@angular/common';
import { LayoutService } from '../../services/layout.service';
import { StyleClassModule } from 'primeng/styleclass';
import { MenuModule } from 'primeng/menu';

@Component({
	selector: 'app-header',
	imports: [
		CommonModule,
		MenubarModule,
		ButtonModule,
		MenuModule,
		StyleClassModule,
	],
	templateUrl: './header.component.html',
	styleUrl: './header.component.scss',
})
export class HeaderComponent {
	public readonly layoutService: LayoutService = inject(LayoutService);
	public readonly themeService: ThemeService = inject(ThemeService);
	private readonly authService: AuthService = inject(AuthService);

	public onLogoutClick(): void {
		this.authService.logout();
	}

	public toggleDarkMode(): void {
		this.themeService.toggleTheme();
	}
}
