DROP DATABASE IF EXISTS Boulangerie;

CREATE DATABASE IF NOT EXISTS Boulangerie;

# ---------------------- Table general ----------------------------------
CREATE TABLE IF NOT EXISTS Boulangerie.Tva (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    Valeur DECIMAL(5, 2) NOT NULL
);

CREATE TABLE IF NOT EXISTS Boulangerie.Groupe (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,

    Nom VARCHAR(300) NOT NULL,
    Adresse VARCHAR(800) NOT NULL,
    Prefix CHAR(3) NOT NULL,
    ConnexionBloquer TINYINT(1) NOT NULL DEFAULT 0
);

# ---------------------- Table 1 relation ----------------------------------
CREATE TABLE IF NOT EXISTS Boulangerie.Client (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT, 
    IdGroupe INT NOT NULL,

    IdPublic CHAR(36) NOT NULL,
    Nom VARCHAR(300) NOT NULL,
    Mail VARCHAR(250) NULL,
    Telephone VARCHAR(20) NULL,
    Login VARCHAR(30) NULL,
    Mdp VARCHAR(300) NULL,
    Adresse VARCHAR(800) NOT NULL,
    AdresseFacturation VARCHAR(800) NOT NULL,
    InfoComplementaire VARCHAR(1000) NULL,
    ConnexionBloquer TINYINT(1) NOT NULL DEFAULT 1,

    FOREIGN KEY (IdGroupe) REFERENCES Groupe (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.Vehicule (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT, 
    IdGroupe INT NOT NULL,

    IdPublic CHAR(36) NOT NULL,
    Nom VARCHAR(100) NOT NULL,
    Immatriculation VARCHAR(15) NOT NULL,
    InfoComplementaire VARCHAR(1000) NULL,
    EstSupprimer TINYINT(1) NOT NULL DEFAULT 0,

    FOREIGN KEY (IdGroupe) REFERENCES Groupe (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.Categorie (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT, 
    IdGroupe INT NOT NULL,

    IdPublic CHAR(36) NOT NULL,

    Nom VARCHAR(300) NOT NULL,
    EstSupprimer TINYINT(1) NOT NULL DEFAULT 0,

    FOREIGN KEY (IdGroupe) REFERENCES Groupe (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.Fournisseur (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT, 
    IdGroupe INT NOT NULL,

    IdPublic CHAR(36) NOT NULL,

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

    IdPublic CHAR(36) NOT NULL,
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

    IdPublic CHAR(36) NOT NULL,
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

    IdPublic CHAR(36) NOT NULL,
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

CREATE TABLE IF NOT EXISTS Boulangerie.Livraison (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    IdVehicule INT NOT NULL,
    IdUtilisateur INT NOT NULL,
    IdPublic CHAR(36) NOT NULL,

    Numero CHAR(20) NOT NULL,
    Frais DECIMAL(10, 2) NOT NULL DEFAULT 0,
    Date DATE NOT NULL,

    FOREIGN KEY (IdVehicule) REFERENCES Vehicule (Id),
    FOREIGN KEY (IdUtilisateur) REFERENCES Utilisateur (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.Commande (
    Id INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    IdGroupe INT NOT NULL,
    IdClient INT NULL,
    IdLivraison INT NULL,

    Numero VARCHAR(15) NOT NULL,
    PrixTotalHT DECIMAL(10, 2),
    EstLivraison TINYINT(1) NOT NULL DEFAULT 0,

    ordreLivraison INT NULL,

    DateCreation DATETIME NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    DatePourLe DATETIME NOT NULL,
    DateValidation DATETIME NULL,
    DatLivraison DATETIME NULL,
    DateAnnulation DATETIME NULL,

    FOREIGN KEY (IdClient) REFERENCES Client (Id),
    FOREIGN KEY (IdGroupe) REFERENCES Groupe (Id),
    FOREIGN KEY (IdLivraison) REFERENCES Livraison (Id)
);

CREATE TABLE IF NOT EXISTS Boulangerie.ProduitCommande (
    IdProduit INT NOT NULL,
    IdCommande INT NOT NULL,

    Quantite INT NOT NULL,
    PrixHT DECIMAL(8, 2) NOT NULL,

    PRIMARY KEY(IdProduit, IdCommande),

    FOREIGN KEY (IdProduit) REFERENCES Produit (Id),
    FOREIGN KEY (IdCommande) REFERENCES Commande (Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Boulangerie.Recette (
    IdIngredient INT NOT NULL,
    IdProduit INT NOT NULL,

    Quantite DECIMAL(10, 3) NOT NULL DEFAULT 0,

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

INSERT INTO Boulangerie.Groupe (Id, Nom, Adresse, Prefix) VALUES (1, "Groupe 1", "Adresse groupe 1", "Gup");
INSERT INTO Boulangerie.Client (Id, IdPublic, IdGroupe, Nom, Adresse, AdresseFacturation) 
VALUES 
(1, "0a1ea0c8-c898-4c54-8492-44d19b4ebcae", 1, "Grand hotel", "13 rue de paris", "13 rue de paris"),
(2, "a78ef710-dbe6-4704-8ada-7cf57c0b55e4", 1, "Ecole chocolaterie", "16 avenue Gustave Rocher", "16 avenue Gustave Rocher"),
(3, "d41a0aa9-7f45-4e11-9e5c-3c1ef2bdd236", 1, "Ecole maternelle Marie Brochet", "387 bd Gregoire Stark", "387 bd Gregoire Stark"),
(4, "b6281099-b6b0-44e6-82c9-45a02f457efc", 1, "Point chaud", "1 rue Estelle Tripot", "1 rue Estelle Tripot");

INSERT INTO Boulangerie.Utilisateur (Id, IdGroupe, IdPublic, Nom, Prenom, Mail, Telephone, Mdp, EstAdmin) VALUES
(1, 1, "d41a0aa9-7f45-4e11-9e5c-3c1ef2bdd236", "Peyrachon", "Nicolas", "admin@mail.com", "0712345678", "vOba9D9fgUAtZLmjLOEj8g==$topujlqNGMnRZUBb70rk8GC+0H7lMRx8+dXtMH2SYT0=", 1),
(2, 1, "0a71cc4c-165b-46a3-b864-057b558c7bbe", "Dupont", "Rémi", "remi@mail.com", "0712345678", "vOba9D9fgUAtZLmjLOEj8g==$topujlqNGMnRZUBb70rk8GC+0H7lMRx8+dXtMH2SYT0=", 1),
(3, 1, "88c29f06-4725-43fd-bfb3-5ffd7b50dc7e", "Preland", "Ambre", "ambre@mail.com", "0712345678", "vOba9D9fgUAtZLmjLOEj8g==$topujlqNGMnRZUBb70rk8GC+0H7lMRx8+dXtMH2SYT0=", 1);

INSERT INTO Boulangerie.Fournisseur (
    Id, IdGroupe, IdPublic, 
    Nom, Adresse, Telephone, Mail
) VALUES
(
    1, 1, "f9bb96e0-45dd-4b41-b0c9-cf95adb1643b", 
    "Les Blés de France", "30 rue Roger Artman", "0712345678", "ble.france@mail.com"
),
(
    2, 1, "0b68d9fd-899a-44f1-b3f5-4d3c0c879580", 
    "Maison Douceur", "210 rue fournier", "0712345678", "maison.douceur@mail.com"
),
(
    3, 1, "76dae8c3-3b98-448e-9503-4be9a242dd93", 
    "Cacao & Compagnie", "90 avenue des pains", "0712345678", "cacao.compagnie@mail.com"
);

INSERT INTO Boulangerie.Vehicule (Id, IdGroupe, Nom, IdPublic, Immatriculation, InfoComplementaire) VALUES 
(1, 1, "Renault 4", "d41a0aa9-7f45-4e11-9e5c-3c1ef2bdd236", "GA-849-NT", null),
(2, 1, "Clio 6", "d41a0aa9-7f45-4e11-9e5c-3c1ef2bad237", "ML-650-PK", "En réparation"),
(3, 1, "Fourgon", "0a71cc4c-165b-46a3-b864-057b558c7bbe", "DE-439-XH", null);

INSERT INTO Boulangerie.Tva (Id, Valeur) 
VALUES 
(1, 5.5), (2, 10), (3, 20), (4, 8.5), (5, 2.10), (6, 1.75), (7, 1.05);

INSERT INTO Boulangerie.Ingredient (Id, IdGroupe, IdPublic, Nom, CodeInterne, Stock, StockAlert)
VALUES 
(1, 1, "d41a0aa9-7f45-4e11-9e5c-3c1ef2bdd236", "Chocolat", "cho1", 10, 0),
(2, 1, "43192df3-6bbe-46d5-8996-f8ba4091aa4f", "Sucre", "su", 32, 10),
(3, 1, "0a71cc4c-165b-46a3-b864-057b558c7bbe", "Farine", "T80", 100, 10),
(4, 1, "6fbf7473-7c90-4778-a23c-81d064acb510", "Sel", "S1", 54, 5),
(5, 1, "bef33b2f-e120-4486-931a-d3d1caca8545", "Beurre", "B0", 232, 10),
(6, 1, "86f24e3e-6c49-4999-b20a-192c5e5168f3", "Oeuf", "E2", 11, 0),
(7, 1, "6c51eb82-0078-4d64-be14-1b23123d4466", "Levure", "LC", 392, 20);

INSERT INTO Boulangerie.Categorie (Id, IdGroupe, IdPublic, Nom) VALUES
(1, 1, "9b0eb25d-a78d-4af6-ba7b-90f7f50e09b3", "Pain"),
(2, 1, "a21cef71-33a7-4945-8174-7c67be4104e1", "Gateau"),
(3, 1, "b4cd8572-a669-4aaf-97fd-8084146820c5", "Viennoiserie"),
(4, 1, "6fbf7473-7c90-4778-a23c-81d064acb510", "Sandwich"),
(5, 1, "d41a0aa9-7f45-4e11-9e5c-3c1ef2bdd236", "Dessert");

INSERT INTO Boulangerie.Produit (
    Id, IdGroupe, IdCategorie, IdTva, IdPublic, Nom,
    PrixHT, Alergene, CodeInterne, Poids, Stock, StockAlert
) VALUES
(
    1, 1, 1, 3, "fec52668-8da7-4e53-bdc0-540ec63bb9b5", "Pain blanc",
    1.20, "Gluten", "PB", null, 10, 2
),
(
    2, 1, 3, 3, "c8b114a3-8ab6-485d-9aef-b864371f8504", "Eclair au chocolat",
    3, "", "EC", null, 5, 1
),
(
    3, 1, 1, 3, "6fbf7473-7c90-4778-a23c-81d064acb510", "Pain complet",
    1.80, "Gluten", "PC", 100, 43, 10
),
(
    4, 1, 5, 3, "9b0eb25d-a78d-4af6-ba7b-90f7f50e09b3", "Macaron chocolat",
    2.20, "", "Mac-choc", 50, 200, 0
),
(
    5, 1, 1, 3, "86f24e3e-6c49-4999-b20a-192c5e5168f3", "Brioche",
    4, "", "B32", 200, 200, 0
);

INSERT INTO Boulangerie.Commande (Id, IdGroupe, IdClient, PrixTotalHT, Numero, DatePourLe, EstLivraison, DateValidation)
VALUES 
(1, 1, 1, 10.50, "Gup447390279310", "2025-09-16", 1, "2025-09-16"),
(2, 1, 1, 30, "Gup447390279710", "2025-09-16", 1, "2025-09-16"),
(3, 1, null, 30, "Gup447390271710", "2025-09-16", 0, "2025-09-16");

INSERT INTO Boulangerie.ProduitCommande (IdProduit, IdCommande, Quantite, PrixHT)
VALUES
(1, 1, 10, 1.20), (2, 1, 2, 3),
(1, 2, 10, 1.20), (2, 2, 2, 3),
(1, 3, 10, 1.20), (2, 3, 2, 3);
