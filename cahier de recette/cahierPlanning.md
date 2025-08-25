# Cahier de recette planning

# Scénario Ajouter une commande

## Fonctionnalité
Ajouter une commande

## Scénario
Ajouter une commande

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `planning`
- Le filtre doit être sur `mois`

## Etapes
1) Cliquer sur un jour dans le calendrier
2) Cliquer sur le bouton `Ajouter`
3) Complèter le formulaire
4) Cliquer sur le bouton `Ajouter`

## Résultat attendu
- Le modal se ferme

# Scénario Modifier une commande

## Fonctionnalité
Modifier une commande

## Scénario
Modifier une commande

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `planning`
- Le filtre doit être sur `semaine` ou `jour`

## Etapes
1) Cliquer sur une commande
2) Modifier la commande
3) Cliquer sur le bouton `Modifier`

## Résultat attendu
- Le modal se ferme
- La commande s'actualise

## Résultat erreur
- Le modal reste ouvert
- Un message d'erreur apparait

# Scénario Exporter des commandes

## Fonctionnalité
Exporter des commandes

## Scénario
Exporter des commandes

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `catégorie`

## Etapes
1) Cliquer le bouton en haut avec une flèche qui pointe vers le bas

## Résultats attendus
- Un fichier est téléchargé

# Scénario Télécharger facture commande

## Fonctionnalité
Télécharger facture commande

## Scénario
Télécharger facture commande

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `planning`
- L'utilisateur doit être sur le filtre `semaine` ou `jour`
- Il doit y avoir une commande

## Etapes
1) Cliquer sur le bouton avec une flèche qui pointe vers le bas de la commande
2) Donner l'interval de date souhaité

## Résultats attendus
- Un fichier est téléchargé

# Scénario Programmer une livraison

## Fonctionnalité
Programmer une livraison

## Scénario
Programmer une livraison

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `planning`
- L'utilisateur doit être sur le filtre `semaine` ou `jour`
- Il doit y avoir une commande validé
- Il doit y avoir un véhicule
- Il doit y avoir un employé

## Etapes
1) Cliquer sur le bouton avec l'icon de "camion"
2) Choisir un conducteur
3) Choisir un véhicule
4) Drag and drop les commandes souhaitées à droite

## Résultats attendus
- Le modal se ferme
- Un message de confirmation apparait

## Résultat erreur
- Le modal reste ouvert
- Un message d'erreur apparait