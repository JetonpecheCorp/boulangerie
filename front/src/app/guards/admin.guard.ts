import { CanActivateFn } from '@angular/router';
import { environment } from '../../environments/environment';
import { ERole } from '@enum/ERole';

export const adminGuard: CanActivateFn = (route, state) => 
{ 
  return environment.utilisateur && environment.utilisateur.role == ERole.Admin;
};
