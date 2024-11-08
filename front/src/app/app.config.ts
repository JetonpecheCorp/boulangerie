import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideToastr } from 'ngx-toastr';
import { AuthentificationService } from '../service/Authentification.service';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import {MatToolbarModule} from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS, MatFormFieldDefaultOptions, MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ThemeService } from '@service/ThemeService.Service';
import { ReactiveFormsModule } from '@angular/forms';
import { IngredientService } from '@service/Ingredient.service';
import { ProduitService } from '@service/Produit.service';
import { TvaService } from '@service/Tva.service';
import { CategorieService } from '@service/Categorie.service';
import { JwtInterceptor } from '../interceptor/jwt.interceptor';
import { RecetteService } from '@service/Recette.service';
import { VehiculeService } from '@service/Vehicule.service';

const matInput: MatFormFieldDefaultOptions = {
  appearance: 'outline',
  subscriptSizing: 'dynamic'
};

export const appConfig: ApplicationConfig = {
  providers: [
    importProvidersFrom(
      MatCardModule, MatButtonModule,
      MatFormFieldModule, MatInputModule, 
      MatProgressSpinnerModule, MatDialogModule, 
      MatToolbarModule, MatIconModule, 
      MatSelectModule, MatCheckboxModule, 
      MatRadioModule, MatTableModule, 
      MatSortModule, MatPaginatorModule, 
      MatTooltipModule, ReactiveFormsModule
    ),
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

    { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: matInput }
  ]
};
