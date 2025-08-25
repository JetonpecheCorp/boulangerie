# Cahier de recette employé

# Scénario Ajouter un employé

## Fonctionnalité
Ajouter un employé

## Scénario
Ajouter un employé

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `employé`

## Etapes
1) Cliquer le bouton `Ajouter un(e) employé(e)`
2) Completer le formulaire
3) Cliquer sur le bouton `Ajouter`

## Résultats attendus
- Le modal se ferme
- Le tableau s'actualise

# Scénario Importer un employé

## Fonctionnalité
Importer un employé

## Scénario
Importer un employé

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `employé`

## Etapes
1) Cliquer le bouton `Importer les employés`
2) Cliquer sur le bouton `Choisissez votre fichier (CSV)`
3) Cliquer sur le bouton `Importer`

### Entête des colonnes CSV
nom *;prénom *;mail *;mot de passe *;téléphone;admin

\* => obligatoire 

## Résultats attendus
- Le modal se ferme
- Le tableau s'actualise

## Résultats erreur
- Les erreurs s'affiche s'il y en a
- Le modal reste ouvert

# Scénario Exporter un employé

## Fonctionnalité
Exporter un employé

## Scénario
Exporter un employé

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `employé`

## Etapes
1) Cliquer le bouton `Exporter les employés`

## Résultats attendus
- Si il y a des client un fichier se télécharge
- Si il n'y a pas de client un message en bleu en haut a droite de l'écran apparait et vous l'indique

# Scénario Modifier un employé

## Fonctionnalité
Modifier un employé

## Scénario
Modifier un employé

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `employé`

## Etapes
1) Cliquer sur le bouton à droite dans le tableau avec l'icon du "crayon"
2) Changer les informations souhaitées
3) Cliquer sur le bouton `Modifier`

## Résultats attendus
- Le modal se ferme
- Le tableau s'actualise
