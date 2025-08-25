import { CanActivateFn } from '@angular/router';
import { environment } from '../../environments/environment';

export const adminGuard: CanActivateFn = (route, state) => 
{ 
  return environment.utilisateur && environment.utilisateur.role == "admin";
};
