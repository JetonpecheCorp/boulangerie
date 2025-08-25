# Cahier de recette

# Scénario Connexion

## Fonctionnalité
Connexion à son compte utilisateur

## Scénario 1
Se connecter avec des données valides

## Scénario 2
Se connecter avec des données invalides

## Scénario 3
Completer partiellement le formulaire de connexion

## Prérequis
L'utilisateur doit se rendre l'application

## Etapes
1) Aller sur la l'application
2) Completer le formulaire suivant le scénario choisi
3) Cliquer sur le bouton "Connexion"

## Resultat attendus scénario 1
L'application doit vous redirigez sur la page `planning`

## Resultat attendus scénario 2
L'application doit vous affiche un message d'erreur

## Resultat attendus scénario 3
L'application doit vous affiche un message en dessous des inputs non rempli et le bouton `connexion` ne fait rien

## Fonctionnalité
Export des produits sous format excel (xlsx)

## Scénario
Exporter les produits de la base de données

## Prérequis
L'utilisateur doit être connecté et sur la page `produit`

## Etapes
1) Aller sur la page `produit`
2) Cliquer sur le bouton `Exporter les produits`

## Résultats attendus
Un fichier excel qui contient la liste des produits se télécharge

```
Exemple de scénario de test pour la création d'une commande :

Fonctionnalité : Création d'une commande

Scénario : Créer une nouvelle commande avec des données valides.

Prérequis : L'utilisateur est connecté en tant que "responsable boulangerie".

Étapes :

Aller sur la page de création de commande.

Remplir le formulaire avec un client existant et des produits.

Cliquer sur le bouton "Enregistrer".

Résultats attendus :

L'application affiche un message de confirmation ("Commande créée avec succès").

La page est redirigée vers la liste des commandes.

La nouvelle commande apparaît dans la liste avec le statut "En cours de préparation".

Un test de sécurité est exécuté pour s'assurer qu'un utilisateur de type "livreur" ne peut pas accéder à cette page.
```