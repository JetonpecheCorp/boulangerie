DROP DATABASE Boulangerie;

CREATE DATABASE IF NOT EXISTS Boulangerie;

# ---------------------- Table general ----------------------------------
CREATE TABLE IF NOT EXISTS Boulangerie.Tva (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    Valeur DECIMAL(5, 2) NOT NULL
);

CREATE TABLE IF NOT EXISTS Boulangerie.UtilisateurAdmin (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    IdPublic BINARY(16) NOT NULL,

    Nom VARCHAR(200) NOT NULL,
    Prenom VARCHAR(200) NOT NULL,
    Mail VARCHAR(250) NOT NULL,
    Mdp VARCHAR(300) NOT NULL,
    Telephone VARCHAR(20) NULL
);

CREATE TABLE IF NOT EXISTS Boulangerie.Groupe (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,

    Nom VARCHAR(300) NOT NULL,
    Adresse VARCHAR(800) NOT NULL,
    ConnexionBloquer TINYINT(1) DEFAULT 0
);

# ---------------------- Table 1 relation ----------------------------------

CREATE TABLE IF NOT EXISTS Boulangerie.Planning (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT, 

    Date DATE NOT NULL
);

CREATE TABLE IF NOT EXISTS Boulangerie.Client (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT, 
    IdGroupe INT NOT NULL,

    IdPublic BINARY(16) NOT NULL,
    Nom VARCHAR(300) NOT NULL,
    Mail VARCHAR(250) NULL,
    Telephone VARCHAR(20) NULL,
    Adresse VARCHAR(800) NOT NULL,
    AdresseFacturation VARCHAR(800) NOT NULL,
    InfoComplementaire VARCHAR(1000) NULL,

    FOREIGN KEY (IdGroupe) REFERENCES Groupe (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.Vehicule (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT, 
    IdGroupe INT NOT NULL,

    IdPublic BINARY(16) NOT NULL,
    Immatriculation VARCHAR(15) NOT NULL,
    InfoComplementaire VARCHAR(1000) NULL,

    FOREIGN KEY (IdGroupe) REFERENCES Groupe (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.Categorie (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT, 
    IdGroupe INT NOT NULL,

    IdPublic BINARY(16) NOT NULL,

    Nom VARCHAR(300) NOT NULL,
    EstSupprimer TINYINT(1) NOT NULL DEFAULT 0,

    FOREIGN KEY (IdGroupe) REFERENCES Groupe (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.Fournisseur (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT, 
    IdGroupe INT NOT NULL,

    IdPublic BINARY(16) NOT NULL,

    Nom VARCHAR(300) NOT NULL,
    Adresse VARCHAR(800) NULL,
    Telephone VARCHAR(20) NULL,
    Mail VARCHAR(250) NULL,

    EstSupprimer TINYINT(1) NOT NULL DEFAULT 0,
    DateCreation DATE NOT NULL DEFAULT (CURRENT_DATE),
    DateModification DATE NULL,

    FOREIGN KEY (IdGroupe) REFERENCES Groupe (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.Ingredient (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT, 
    IdGroupe INT NOT NULL,

    IdPublic BINARY(16) NOT NULL,
    Nom VARCHAR(200) NOT NULL,
    CodeInterne VARCHAR(100) NULL,

    Stock DECIMAL(20, 3) NOT NULL DEFAULT 0,
    StockAlert DECIMAL(20, 3) NOT NULL DEFAULT 0,

    EstSupprimer TINYINT(1) NOT NULL DEFAULT 0,

    FOREIGN KEY (IdGroupe) REFERENCES Groupe (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.Utilisateur (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    IdGroupe INT NOT NULL,

    IdPublic BINARY(16) NOT NULL,
    Nom VARCHAR(200) NOT NULL,
    Prenom VARCHAR(200) NOT NULL,
    Mail VARCHAR(250) NOT NULL,
    Mdp VARCHAR(300) NOT NULL,
    Telephone VARCHAR(20) NULL,

    EstAdmin TINYINT(1) NOT NULL DEFAULT 0,

    FOREIGN KEY (IdGroupe) REFERENCES Groupe (Id)
);

# ---------------------- Table relation ----------------------------------

CREATE TABLE IF NOT EXISTS Boulangerie.Produit (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    IdGroupe INT NOT NULL,
    IdCategorie INT NOT NULL,
    IdTva INT NOT NULL,

    IdPublic BINARY(16) NOT NULL,
    Nom VARCHAR(300) NOT NULL,
    PrixHT DECIMAL(8, 2) NOT NULL,
    Alergene VARCHAR (5000) NULL,
    CodeInterne VARCHAR(100) NULL,
    Poids DECIMAL(5, 2) NULL,

    EstSupprimer TINYINT(1) NOT NULL DEFAULT 0,

    DateCreation DATE NOT NULL DEFAULT (CURRENT_DATE),
    DateModification DATE NULL,

    Stock DECIMAL(20, 3) NOT NULL DEFAULT 0,
    StockAlert DECIMAL(20, 3) NOT NULL DEFAULT 0,

    FOREIGN KEY (IdGroupe) REFERENCES Groupe (Id),
    FOREIGN KEY (IdCategorie) REFERENCES Categorie (Id),
    FOREIGN KEY (IdTva) REFERENCES Tva (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.PlanningProduitUtilisateur (
    IdPlanning INT NOT NULL,
    IdProduit INT NOT NULL,

    IdUtilisateur INT NULL,
    Quantite DECIMAL(20, 3) NOT NULL,

    PRIMARY KEY (IdPlanning, IdProduit),

    FOREIGN KEY (IdProduit) REFERENCES Produit (Id),
    FOREIGN KEY (IdPlanning) REFERENCES Planning (Id),
    FOREIGN KEY (IdUtilisateur) REFERENCES Utilisateur (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.Livraison (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    IdVehicule INT NOT NULL,
    IdClient INT NOT NULL,

    Numero CHAR(20) NOT NULL,
    Frais DECIMAL(10, 2) NOT NULL DEFAULT 0,
    Date DATE NOT NULL,

    FOREIGN KEY (IdVehicule) REFERENCES Vehicule (Id),
    FOREIGN KEY (IdClient) REFERENCES Client (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.ProduitLivraison (
    IdProduit INT NOT NULL,
    IdLivraison INT NOT NULL,

    PrixHT DECIMAL(8, 2) NOT NULL,
    Poids DECIMAL(5, 2) NULL,
    Tva DECIMAL(5, 2) NOT NULL,
    Quantite DECIMAL(20, 3) NOT NULL,

    PRIMARY KEY(IdProduit, IdLivraison),

    FOREIGN KEY (IdProduit) REFERENCES Produit (Id),
    FOREIGN KEY (IdLivraison) REFERENCES Livraison (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.Recette (
    IdIngredient INT NOT NULL,
    IdProduit INT NOT NULL,

    Quantite DECIMAL(10, 3),

    PRIMARY KEY (IdIngredient, IdProduit),

    FOREIGN KEY (IdProduit) REFERENCES Produit (Id),
    FOREIGN KEY (IdIngredient) REFERENCES Ingredient (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.FournisseurIngredient (
    IdFournisseur INT NOT NULL,
    IdIngredient INT NOT NULL,

    PRIMARY KEY (IdFournisseur, IdIngredient),

    FOREIGN KEY (IdFournisseur) REFERENCES Fournisseur (Id),
    FOREIGN KEY (IdIngredient) REFERENCES Ingredient (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.FournisseurProduit (
    IdFournisseur INT NOT NULL,
    IdProduit INT NOT NULL,

    PRIMARY KEY (IdFournisseur, IdProduit),

    FOREIGN KEY (IdFournisseur) REFERENCES Fournisseur (Id),
    FOREIGN KEY (IdProduit) REFERENCES Produit (Id)
);

INSERT INTO Boulangerie.Groupe (Id, Nom, Adresse) VALUES (1, "Groupe 1", "Adresse groupe 1");

INSERT INTO Boulangerie.Tva (Id, Valeur) 
VALUES 
(1, 5.5), (2, 10), (3, 20), (4, 8.5), (5, 2.10), (6, 1.75), (7, 1.05);