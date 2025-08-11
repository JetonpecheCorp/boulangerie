# Cahier de recette client

# Scénario Ajouter un client

## Fonctionnalité
Ajouter un client

## Scénario
Ajouter un client

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `client`

## Etapes
1) Cliquer le bouton `Ajouter une client`
2) Completer le formulaire avec au mon le nom et l'adresse
3) Cliquer sur le bouton `Ajouter`

## Résultats attendus
- Le modal se ferme
- Le tableau s'actualise

# Scénario Importer un client

## Fonctionnalité
Importer un client

## Scénario
Importer un client

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `client`

## Etapes
1) Cliquer le bouton `Importer des client`
2) Cliquer sur le bouton `Choisissez votre fichier (CSV)`
3) Cliquer sur le bouton `Importer`

### Entête des colonnes CSV
nom *;adresse *;adresse de facturation *;mail;téléphone

\* => obligatoire 

## Résultats attendus
- Le modal se ferme
- Le tableau s'actualise

## Résultats erreur
- Les erreurs s'affiche s'il y en a
- Le modal reste ouvert

# Scénario Exporter un client

## Fonctionnalité
Exporter un client

## Scénario
Exporter un client

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `client`

## Etapes
1) Cliquer le bouton `Exporter les clients`

## Résultats attendus
- Si il y a des client un fichier se télécharge
- Si il n'y a pas de client un message en bleu en haut a droite de l'écran apparait et vous l'indique

# Scénario Modifier un client

## Fonctionnalité
Modifier un client

## Scénario
Modifier un client

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `client`

## Etapes
1) Cliquer sur le bouton à droite dans le tableau avec l'icon du "crayon"
2) Changer les informations souhaitées avec au moins un nom et une adresse
3) Cliquer sur le bouton `Modifier`

## Résultats attendus
- Le modal se ferme
- Le tableau s'actualise
