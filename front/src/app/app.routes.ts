import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: "", 
        loadComponent: () => import('./pages/login/login.component').then(x => x.LoginComponent), 
        title: "Connexion" 
    },
    {
        path: "ingredient",
        loadComponent: () => import('./pages/ingredient/ingredient.component').then(x => x.IngredientComponent),
        title: "Ingredient"
    },
    {
        path: "produit",
        loadComponent: () => import('./pages/produit/produit.component').then(x => x.ProduitComponent),
        title: "Produit"
    }
];
