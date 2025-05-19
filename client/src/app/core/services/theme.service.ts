import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ThemeService {
	private readonly themeKey = 'preferred-theme';
	private currentTheme = new BehaviorSubject<'light' | 'dark'>('light');
	theme$ = this.currentTheme.asObservable();

	constructor() {
		const saved = localStorage.getItem(this.themeKey) as
			| 'light'
			| 'dark'
			| null;
		this.applyTheme(saved ?? 'light');
	}

	toggleTheme() {
		const newTheme = this.currentTheme.value === 'light' ? 'dark' : 'light';
		this.applyTheme(newTheme);
	}

	private applyTheme(theme: 'light' | 'dark') {
		this.currentTheme.next(theme);
		localStorage.setItem(this.themeKey, theme);
		const html = document.documentElement;

		theme === 'dark' ? html.classList.add('dark-theme') : html.classList.remove('dark-theme');
	}

	isDarkMode() {
		return this.currentTheme.value === 'dark';
	}
}
