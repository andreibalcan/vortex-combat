import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../../shared/services/auth/auth.service';

export const roleGuard = (allowedRoles: string[]): CanActivateFn => {
  return () => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (!authService.isAuthenticated()) {
      router.navigate(['/login']);
      return false;
    }

    const hasRequiredRole = allowedRoles.some((role) =>
      authService.hasRole(role)
    );

    if (!hasRequiredRole) {
      router.navigate(['/login']); // TODO: Redirect to an error page.
      return false;
    }

    return true;
  };
};
