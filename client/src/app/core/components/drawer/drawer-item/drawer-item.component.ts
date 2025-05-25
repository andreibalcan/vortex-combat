import { CommonModule } from '@angular/common';
import {
	Component,
	HostBinding,
	inject,
	input,
	OnInit,
	effect,
	booleanAttribute,
} from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { RippleModule } from 'primeng/ripple';
import {
	animate,
	state,
	style,
	transition,
	trigger,
} from '@angular/animations';
import { LayoutService } from '../../../services/layout.service';
import { MenuItem } from 'primeng/api';
import { filter, Subscription } from 'rxjs';

@Component({
	selector: '[app-drawer-item]',
	imports: [CommonModule, RouterModule, RippleModule],
	templateUrl: './drawer-item.component.html',
	styleUrl: './drawer-item.component.scss',
	animations: [
		trigger('children', [
			state(
				'collapsed',
				style({
					height: '0',
				}),
			),
			state(
				'expanded',
				style({
					height: '*',
				}),
			),
			transition(
				'collapsed <=> expanded',
				animate('400ms cubic-bezier(0.86, 0, 0.07, 1)'),
			),
		]),
	],
	providers: [LayoutService],
})
export class DrawerItemComponent implements OnInit {
	public item = input.required<MenuItem>();
	public index = input.required<number>();
	public root = input(false, { transform: booleanAttribute });
	// Host binding as a regular property
	@HostBinding('class.layout-root-menuitem') rootHostBinding = false;

	public parentKey = input<string>();

	private active = false;

	private menuSourceSubscription: Subscription;
	private menuResetSubscription: Subscription;

	public key: string = '';

	public readonly router = inject(Router);
	public readonly layoutService = inject(LayoutService);

	constructor() {
		this.menuSourceSubscription = this.layoutService.menuSource$.subscribe(
			(value) => {
				Promise.resolve(null).then(() => {
					if (value.routeEvent) {
						this.active =
							value.key === this.key ||
							value.key.startsWith(this.key + '-')
								? true
								: false;
					} else {
						if (
							value.key !== this.key &&
							!value.key.startsWith(this.key + '-')
						) {
							this.active = false;
						}
					}
				});
			},
		);

		this.menuResetSubscription = this.layoutService.resetSource$.subscribe(
			() => {
				this.active = false;
			},
		);

		this.router.events
			.pipe(filter((event) => event instanceof NavigationEnd))
			.subscribe((params) => {
				if (this.item().routerLink) {
					this.updateActiveStateFromRoute();
				}
			});

		effect(() => {
			this.rootHostBinding = this.root();
		});
	}

	ngOnInit() {
		this.key = this.parentKey()
			? this.parentKey() + '-' + this.index()
			: String(this.index());

		if (this.item().routerLink) {
			this.updateActiveStateFromRoute();
		}
	}

	updateActiveStateFromRoute() {
		let activeRoute = this.router.isActive(this.item().routerLink[0], {
			paths: 'exact',
			queryParams: 'ignored',
			matrixParams: 'ignored',
			fragment: 'ignored',
		});

		if (activeRoute) {
			this.layoutService.onMenuStateChange({
				key: this.key,
				routeEvent: true,
			});
		}
	}

	itemClick(event: Event) {
		// avoid processing disabled items
		if (this.item().disabled) {
			event.preventDefault();
			return;
		}

		// execute command
		if (this.item().command) {
			this.item().command?.({ originalEvent: event, item: this.item() });
		}

		// toggle active state
		if (this.item().items) {
			this.active = !this.active;
		}

		this.layoutService.onMenuStateChange({ key: this.key });
	}

	get submenuAnimation() {
		return this.root()
			? 'expanded'
			: this.active
				? 'expanded'
				: 'collapsed';
	}

	@HostBinding('class.active-menuitem')
	get activeClass() {
		return this.active && !this.root();
	}

	ngOnDestroy(): void {
		if (this.menuSourceSubscription) {
			this.menuSourceSubscription.unsubscribe();
		}

		if (this.menuResetSubscription) {
			this.menuResetSubscription.unsubscribe();
		}
	}
}
