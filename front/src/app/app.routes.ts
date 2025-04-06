import { Routes } from '@angular/router';

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
    },
    {
        path: "categorie",
        loadComponent: () => import('./pages/categorie/categorie.component').then(x => x.CategorieComponent),
        title: "Catégorie"
    },
    {
        path: "vehicule",
        loadComponent: () => import('./pages/vehicule/vehicule.component').then(x => x.VehiculeComponent),
        title: "Vehicule"
    },
    {
        path: "fournisseur",
        loadComponent: () => import('./pages/fournisseur/fournisseur.component').then(x => x.FournisseurComponent),
        title: "Fournisseur"
    },
    {
        path: "planning",
        loadComponent: () => import('./pages/test/test.component').then(x => x.TestComponent),
        title: "Planning"
    },
    {
        path: "livraison",
        loadComponent: () => import('./pages/livraison/livraison.component').then(x => x.LivraisonComponent),
        title: "Livraison"
    },
    {
        path: "employe",
        loadComponent: () => import('./pages/utilisateur/utilisateur.component').then(x => x.UtilisateurComponent),
        title: "Employe"
    },
    {
        path: "client",
        loadComponent: () => import('./pages/client/client.component').then(x => x.ClientComponent),
        title: "Client"
    },
    {
        path: "groupe",
        loadComponent: () => import('./pages/groupe/groupe.component').then(x => x.GroupeComponent),
        title: "Groupe"
    }
];
