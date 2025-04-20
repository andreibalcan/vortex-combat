import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { PrimeNG } from 'primeng/config';
import { CardModule } from 'primeng/card';

@Component({
	selector: 'app-root',
	imports: [RouterModule, RouterOutlet, CardModule, ButtonModule],
	templateUrl: './app.component.html',
	styleUrl: './app.component.scss',
})
export class AppComponent {}
