import { CanActivateFn } from '@angular/router';
import { environment } from '../../environments/environment';

export const adminClientGuard: CanActivateFn = (route, state) => 
  environment.utilisateur && (environment.utilisateur.role == "admin" || environment.utilisateur.role == "client");
