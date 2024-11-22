import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideToastr } from 'ngx-toastr';
import { AuthentificationService } from '../service/Authentification.service';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS, MatFormFieldDefaultOptions } from '@angular/material/form-field';
import { ThemeService } from '@service/ThemeService.Service';
import { IngredientService } from '@service/Ingredient.service';
import { ProduitService } from '@service/Produit.service';
import { TvaService } from '@service/Tva.service';
import { CategorieService } from '@service/Categorie.service';
import { JwtInterceptor } from '../interceptor/jwt.interceptor';
import { RecetteService } from '@service/Recette.service';
import { VehiculeService } from '@service/Vehicule.service';
import { FournissseurService } from '@service/Fournisseur.service';

const matInput: MatFormFieldDefaultOptions = {
  appearance: 'outline',
  subscriptSizing: 'dynamic'
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes), 
    provideAnimationsAsync(),
    provideHttpClient(withInterceptors([JwtInterceptor])),
    provideToastr(),

    { provide: LocationStrategy, useClass: HashLocationStrategy },
    { provide: AuthentificationService, useClass: AuthentificationService },
    { provide: ThemeService, useClass: ThemeService },
    { provide: IngredientService, useClass: IngredientService },
    { provide: ProduitService, useClass: ProduitService },
    { provide: TvaService, useClass: TvaService },
    { provide: CategorieService, useClass: CategorieService },
    { provide: RecetteService, useClass: RecetteService },
    { provide: VehiculeService, useClass: VehiculeService },
    { provide: FournissseurService, useClass: FournissseurService },

    { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: matInput }
  ]
};
