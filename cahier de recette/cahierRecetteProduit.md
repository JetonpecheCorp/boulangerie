# Cahier de recette produit

# Scénario Créer un produit

## Fonctionnalité
Creation d'un produit

## Scénario
Créer un nouveau produit avec des données valides

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `produit`

## Etapes
1) Cliquer sur le bouton `Ajouter un produit`
2) Completer le formulaire avec au moins le nom
3) Apuyer sur le bouton `Ajouter`

## Résultats attendus
- Le modal se ferme
- Un message de confirmation vert en haut à droite apparait
- Le tableau s'actualise automatiquement

# Scénario Modifier un produit

## Fonctionnalité
Modifier un produit existant

## Scénario
Modifier un produit avec des données valides

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `produit`

## Etapes
1) Cliquer sur le bouton à droite dans le tableau avec l'icon du "crayon"
2) Ajuster les informations du produit avec des données valides sur le formulaire avec au moins le nom
3) Apuyer sur le bouton `Modifier`

## Résultats attendus
- Le modal se ferme
- Un message de confirmation vert en haut à droite apparait
- Le tableau s'actualise automatiquement

# Scenario Lister les ingredients de la recette du produit

## Fonctionnalité
Lister les ingredients de la recette du produit

## Scénario
Modifier un produit avec des données valides

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `produit`

## Etapes
1) Cliquer sur le bouton à droite dans le tableau avec l'icon "liste"

## Résultats attendus
- Le modal s'ouvre
- Un tableau s'affiche avec liste d'ingredient si il y en a
- Un bouton `Ajouter un ingredient` s'affiche

# Scenario Ajouter un ou des ingredient(s) à la recette du produit

## Fonctionnalité
 Ajouter un ou des ingredient(s)

## Scénario
Ajouter un ou des ingredient(s) à la recette du produit

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `produit`

## Etapes
1) Cliquer sur le bouton à droite dans le tableau avec l'icon "liste"
2) Cliquer sur le bouton `Ajouter un ingredient`
3) Choisissez un ingredient dans la liste et la quantité
4) Appuyer sur le bouton `Ajouter`
4) Appuyer sur le bouton `Ajouter et fermer` 

## Résultats attendus Ajouter
- Le formulaire se vide
- L'ingredient choisi n'ai plus dans la liste

## Résultats attendus Ajouter et fermer
- Le modal se ferme
- Le tableau des ingredients s'actualise

# Scénario Modifier la quantité d'un ingredient

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `produit`

## Etapes
1) Cliquer sur le bouton à droite dans le tableau avec l'icon "liste"
2) Cliquer sur le bouton a droite avec l'icon de "crayon"
3) Saisissez la nouvelle quantité souhaitée
4) Appuyer sur le bouton `Modifier`

## Résultats attendus
- Le modal se ferme
- Le tableau s'actualise avec la nouvelle quantité

# Scénario Supprimer un ingredient

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `produit`

## Etapes
1) Cliquer sur le bouton à droite dans le tableau avec l'icon "liste"
2) Cliquer sur le bouton a droite avec l'icon de "poubelle"
3) Cliquer sur le bouton "oui"

## Résultats attendus
- Le modal se ferme
- Le tableau s'actualise

# Scénario Exporter les produits

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