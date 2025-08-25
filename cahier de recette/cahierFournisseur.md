# Cahier de recette fournisseur

# Scénario Ajouter un fournisseur

## Fonctionnalité
Ajouter un fournisseur

## Scénario
Ajouter un fournisseur

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `fournisseur`

## Etapes
1) Cliquer le bouton `Ajouter une fournisseur`
2) Completer le formulaire avec au mon le nom
3) Cliquer sur le bouton `Ajouter`

## Résultats attendus
- Le modal se ferme
- Le tableau s'actualise

# Scénario Importer un fournisseur

## Fonctionnalité
Importer un fournisseur

## Scénario
Importer un fournisseur

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `fournisseur`

## Etapes
1) Cliquer le bouton `Importer des fournisseurs`
2) Cliquer sur le bouton `Choisissez votre fichier (CSV)`
3) Cliquer sur le bouton `Importer`

### Entête des colonnes CSV
nom *;adresse;mail;téléphone

Le nom est obligatoire

## Résultats attendus
- Le modal se ferme
- Le tableau s'actualise

## Résultats erreur
- Les erreurs s'affiche s'il y en a
- Le modal reste ouvert

# Scénario Exporter un fournisseur

## Fonctionnalité
Exporter un fournisseur

## Scénario
Exporter un fournisseur

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `fournisseur`

## Etapes
1) Cliquer le bouton `Exporter les fournisseurs`

## Résultats attendus
- Si il y a des fournisseur un fichier se télécharge
- Si il n'y a pas de fournisseur un message en bleu en haut a droite de l'écran apparait et vous l'indique

# Scénario Modifier un fournisseur

## Fonctionnalité
Modifier un fournisseur

## Scénario
Modifier un fournisseur

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `fournisseur`

## Etapes
1) Cliquer sur le bouton à droite dans le tableau avec l'icon du "crayon"
2) Changer les informations souhaitées avec au moins un nom
3) Cliquer sur le bouton `Modifier`

## Résultats attendus
- Le modal se ferme
- Le tableau s'actualise

# Scénario Envoyer un mail à un fournisseur

## Fonctionnalité
Envoyer un mail à un fournisseur

## Scénario
Envoyer un mail à un fournisseur

## Prérequis
- L'utilisateur doit être connecté
- L'utilisateur doit être sur la page `fournisseur`
- Le fournisseur doit avoir une adresse mail

## Etapes
1) Cliquer sur le bouton à droite dans le tableau avec l'icon de "enveloppe"
2) Votre boite mail par defaut s'ouvre ou cliquer sur celle que vous voulez

## Résultats attendus
- Aucun
