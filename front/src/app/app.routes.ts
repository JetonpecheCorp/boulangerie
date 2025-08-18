import { Routes } from '@angular/router';
import { adminGuard } from './guards/admin.guard';

export const routes: Routes = [
    {
        path: "", 
        loadComponent: () => import('./pages/login/login.component').then(x => x.LoginComponent), 
        title: "Connexion" 
    },
    {
        path: "reset-mdp", 
        data: { p: null },
        loadComponent: () => import('./pages/reset-mdp/reset-mdp.component').then(x => x.ResetMdpComponent), 
        title: "Reinitialiser le mot de passe" 
    },
    {
        path: "ingredient",
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/ingredient/ingredient.component').then(x => x.IngredientComponent),
        title: "Ingredient"
    },
    {
        path: "produit",
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/produit/produit.component').then(x => x.ProduitComponent),
        title: "Produit"
    },
    {
        path: "categorie",
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/categorie/categorie.component').then(x => x.CategorieComponent),
        title: "Catégorie"
    },
    {
        path: "vehicule",
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/vehicule/vehicule.component').then(x => x.VehiculeComponent),
        title: "Vehicule"
    },
    {
        path: "fournisseur",
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/fournisseur/fournisseur.component').then(x => x.FournisseurComponent),
        title: "Fournisseur"
    },
    {
        path: "planning",
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/test/test.component').then(x => x.TestComponent),
        title: "Planning"
    },
    {
        path: "livraison",
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/livraison/livraison.component').then(x => x.LivraisonComponent),
        title: "Livraison"
    },
    {
        path: "employe",
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/utilisateur/utilisateur.component').then(x => x.UtilisateurComponent),
        title: "Employe"
    },
    {
        path: "client",
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/client/client.component').then(x => x.ClientComponent),
        title: "Client"
    },
    {
        path: "groupe",
        canActivate: [adminGuard],
        loadComponent: () => import('./pages/groupe/groupe.component').then(x => x.GroupeComponent),
        title: "Groupe"
    }
];
