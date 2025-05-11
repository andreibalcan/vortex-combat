import { inject, Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
	providedIn: 'root',
})
export class ToastService {
	private readonly messageService: MessageService = inject(MessageService);

	success(summary: string, detail?: string, life = 3000) {
		this.messageService.add({
			key: 'global',
			severity: 'success',
			summary,
			detail,
			life,
		});
	}

	error(summary: string, detail?: string, life = 3000) {
		this.messageService.add({
			key: 'global',
			severity: 'error',
			summary,
			detail,
			life,
		});
	}

	info(summary: string, detail?: string, life = 3000) {
		this.messageService.add({
			key: 'global',
			severity: 'info',
			summary,
			detail,
			life,
		});
	}

	warn(summary: string, detail?: string, life = 3000) {
		this.messageService.add({
			key: 'global',
			severity: 'warn',
			summary,
			detail,
			life,
		});
	}

	clear() {
		this.messageService.clear('global');
	}
}
