import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideToastr } from 'ngx-toastr';
import { AuthentificationService } from '../service/Authentification.service';
import { HashLocationStrategy, LocationStrategy, registerLocaleData } from '@angular/common';
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
import { CalendarDateFormatter, CalendarModule, CalendarNativeDateFormatter, DateAdapter, DateFormatterParams } from "angular-calendar";
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import localeFr from '@angular/common/locales/fr';

registerLocaleData(localeFr, "fr");

class CustomDateFormat extends CalendarNativeDateFormatter
{
  public override weekViewHour({ date, locale }: DateFormatterParams): string 
  {
    return new Intl.DateTimeFormat(locale, { hour: "numeric", minute: "numeric" }).format(date);
  }
}


const matInput: MatFormFieldDefaultOptions = {
  appearance: 'outline',
  subscriptSizing: 'dynamic'
};

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(
      CalendarModule.forRoot({
        provide: DateAdapter,
        useFactory: adapterFactory
      })
    ),
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes), 
    provideAnimationsAsync(),
    provideHttpClient(withInterceptors([JwtInterceptor])),
    provideToastr(),
    
    { provide: LocationStrategy, useClass: HashLocationStrategy },
    { provide: CalendarDateFormatter, useClass: CustomDateFormat },
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
