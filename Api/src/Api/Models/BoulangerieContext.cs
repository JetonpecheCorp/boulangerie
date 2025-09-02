using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public partial class BoulangerieContext : DbContext
{
    public BoulangerieContext()
    {
    }

    public BoulangerieContext(DbContextOptions<BoulangerieContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorie> Categories { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Commande> Commandes { get; set; }

    public virtual DbSet<Fournisseur> Fournisseurs { get; set; }

    public virtual DbSet<Groupe> Groupes { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Livraison> Livraisons { get; set; }

    public virtual DbSet<Produit> Produits { get; set; }

    public virtual DbSet<ProduitCommande> ProduitCommandes { get; set; }

    public virtual DbSet<Recette> Recettes { get; set; }

    public virtual DbSet<Tva> Tvas { get; set; }

    public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

    public virtual DbSet<Vehicule> Vehicules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Categorie");

            entity.HasIndex(e => e.IdGroupe, "IdGroupe");

            entity.Property(e => e.Nom).HasMaxLength(300);

            entity.HasOne(d => d.IdGroupeNavigation).WithMany(p => p.Categories)
                .HasForeignKey(d => d.IdGroupe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Categorie_ibfk_1");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Client");

            entity.HasIndex(e => e.IdGroupe, "IdGroupe");

            entity.Property(e => e.Adresse).HasMaxLength(800);
            entity.Property(e => e.AdresseFacturation).HasMaxLength(800);
            entity.Property(e => e.InfoComplementaire).HasMaxLength(1000);
            entity.Property(e => e.Mail).HasMaxLength(250);
            entity.Property(e => e.Nom).HasMaxLength(300);
            entity.Property(e => e.Telephone).HasMaxLength(20);

            entity.HasOne(d => d.IdGroupeNavigation).WithMany(p => p.Clients)
                .HasForeignKey(d => d.IdGroupe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Client_ibfk_1");
        });

        modelBuilder.Entity<Commande>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Commande");

            entity.HasIndex(e => e.IdClient, "IdClient");

            entity.HasIndex(e => e.IdGroupe, "IdGroupe");

            entity.HasIndex(e => e.IdLivraison, "IdLivraison");

            entity.Property(e => e.DatLivraison).HasColumnType("datetime");
            entity.Property(e => e.DateAnnulation).HasColumnType("datetime");
            entity.Property(e => e.DateCreation)
                .HasDefaultValueSql("now()")
                .HasColumnType("datetime");
            entity.Property(e => e.DatePourLe).HasColumnType("datetime");
            entity.Property(e => e.DateValidation).HasColumnType("datetime");
            entity.Property(e => e.Numero).HasMaxLength(15);
            entity.Property(e => e.OrdreLivraison).HasColumnName("ordreLivraison");
            entity.Property(e => e.PrixTotalHt)
                .HasPrecision(10, 2)
                .HasColumnName("PrixTotalHT");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Commandes)
                .HasForeignKey(d => d.IdClient)
                .HasConstraintName("Commande_ibfk_1");

            entity.HasOne(d => d.IdGroupeNavigation).WithMany(p => p.Commandes)
                .HasForeignKey(d => d.IdGroupe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Commande_ibfk_2");

            entity.HasOne(d => d.IdLivraisonNavigation).WithMany(p => p.Commandes)
                .HasForeignKey(d => d.IdLivraison)
                .HasConstraintName("Commande_ibfk_3");
        });

        modelBuilder.Entity<Fournisseur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Fournisseur");

            entity.HasIndex(e => e.IdGroupe, "IdGroupe");

            entity.Property(e => e.Adresse).HasMaxLength(800);
            entity.Property(e => e.DateCreation).HasDefaultValueSql("curdate()");
            entity.Property(e => e.Mail).HasMaxLength(250);
            entity.Property(e => e.Nom).HasMaxLength(300);
            entity.Property(e => e.Telephone).HasMaxLength(20);

            entity.HasOne(d => d.IdGroupeNavigation).WithMany(p => p.Fournisseurs)
                .HasForeignKey(d => d.IdGroupe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fournisseur_ibfk_1");

            entity.HasMany(d => d.IdIngredients).WithMany(p => p.IdFournisseurs)
                .UsingEntity<Dictionary<string, object>>(
                    "FournisseurIngredient",
                    r => r.HasOne<Ingredient>().WithMany()
                        .HasForeignKey("IdIngredient")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FournisseurIngredient_ibfk_2"),
                    l => l.HasOne<Fournisseur>().WithMany()
                        .HasForeignKey("IdFournisseur")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FournisseurIngredient_ibfk_1"),
                    j =>
                    {
                        j.HasKey("IdFournisseur", "IdIngredient")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("FournisseurIngredient");
                        j.HasIndex(new[] { "IdIngredient" }, "IdIngredient");
                    });

            entity.HasMany(d => d.IdProduits).WithMany(p => p.IdFournisseurs)
                .UsingEntity<Dictionary<string, object>>(
                    "FournisseurProduit",
                    r => r.HasOne<Produit>().WithMany()
                        .HasForeignKey("IdProduit")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FournisseurProduit_ibfk_2"),
                    l => l.HasOne<Fournisseur>().WithMany()
                        .HasForeignKey("IdFournisseur")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FournisseurProduit_ibfk_1"),
                    j =>
                    {
                        j.HasKey("IdFournisseur", "IdProduit")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("FournisseurProduit");
                        j.HasIndex(new[] { "IdProduit" }, "IdProduit");
                    });
        });

        modelBuilder.Entity<Groupe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Groupe");

            entity.Property(e => e.Adresse).HasMaxLength(800);
            entity.Property(e => e.Nom).HasMaxLength(300);
            entity.Property(e => e.Prefix)
                .HasMaxLength(3)
                .IsFixedLength();
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Ingredient");

            entity.HasIndex(e => e.IdGroupe, "IdGroupe");

            entity.Property(e => e.CodeInterne).HasMaxLength(100);
            entity.Property(e => e.Nom).HasMaxLength(200);
            entity.Property(e => e.Stock).HasPrecision(20, 3);
            entity.Property(e => e.StockAlert).HasPrecision(20, 3);

            entity.HasOne(d => d.IdGroupeNavigation).WithMany(p => p.Ingredients)
                .HasForeignKey(d => d.IdGroupe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Ingredient_ibfk_1");
        });

        modelBuilder.Entity<Livraison>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Livraison");

            entity.HasIndex(e => e.IdUtilisateur, "IdUtilisateur");

            entity.HasIndex(e => e.IdVehicule, "IdVehicule");

            entity.Property(e => e.Frais).HasPrecision(10, 2);
            entity.Property(e => e.Numero)
                .HasMaxLength(20)
                .IsFixedLength();

            entity.HasOne(d => d.IdUtilisateurNavigation).WithMany(p => p.Livraisons)
                .HasForeignKey(d => d.IdUtilisateur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Livraison_ibfk_2");

            entity.HasOne(d => d.IdVehiculeNavigation).WithMany(p => p.Livraisons)
                .HasForeignKey(d => d.IdVehicule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Livraison_ibfk_1");
        });

        modelBuilder.Entity<Produit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Produit");

            entity.HasIndex(e => e.IdCategorie, "IdCategorie");

            entity.HasIndex(e => e.IdGroupe, "IdGroupe");

            entity.HasIndex(e => e.IdTva, "IdTva");

            entity.Property(e => e.Alergene).HasMaxLength(5000);
            entity.Property(e => e.CodeInterne).HasMaxLength(100);
            entity.Property(e => e.DateCreation).HasDefaultValueSql("curdate()");
            entity.Property(e => e.Nom).HasMaxLength(300);
            entity.Property(e => e.Poids).HasPrecision(5, 2);
            entity.Property(e => e.PrixHt)
                .HasPrecision(8, 2)
                .HasColumnName("PrixHT");
            entity.Property(e => e.Stock).HasPrecision(20, 3);
            entity.Property(e => e.StockAlert).HasPrecision(20, 3);

            entity.HasOne(d => d.IdCategorieNavigation).WithMany(p => p.Produits)
                .HasForeignKey(d => d.IdCategorie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Produit_ibfk_2");

            entity.HasOne(d => d.IdGroupeNavigation).WithMany(p => p.Produits)
                .HasForeignKey(d => d.IdGroupe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Produit_ibfk_1");

            entity.HasOne(d => d.IdTvaNavigation).WithMany(p => p.Produits)
                .HasForeignKey(d => d.IdTva)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Produit_ibfk_3");
        });

        modelBuilder.Entity<ProduitCommande>(entity =>
        {
            entity.HasKey(e => new { e.IdProduit, e.IdCommande })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("ProduitCommande");

            entity.HasIndex(e => e.IdCommande, "IdCommande");

            entity.Property(e => e.PrixHt)
                .HasPrecision(8, 2)
                .HasColumnName("PrixHT");

            entity.HasOne(d => d.IdCommandeNavigation).WithMany(p => p.ProduitCommandes)
                .HasForeignKey(d => d.IdCommande)
                .HasConstraintName("ProduitCommande_ibfk_2");

            entity.HasOne(d => d.IdProduitNavigation).WithMany(p => p.ProduitCommandes)
                .HasForeignKey(d => d.IdProduit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProduitCommande_ibfk_1");
        });

        modelBuilder.Entity<Recette>(entity =>
        {
            entity.HasKey(e => new { e.IdIngredient, e.IdProduit })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("Recette");

            entity.HasIndex(e => e.IdProduit, "IdProduit");

            entity.Property(e => e.Quantite).HasPrecision(10, 3);

            entity.HasOne(d => d.IdIngredientNavigation).WithMany(p => p.Recettes)
                .HasForeignKey(d => d.IdIngredient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Recette_ibfk_2");

            entity.HasOne(d => d.IdProduitNavigation).WithMany(p => p.Recettes)
                .HasForeignKey(d => d.IdProduit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Recette_ibfk_1");
        });

        modelBuilder.Entity<Tva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Tva");

            entity.Property(e => e.Valeur).HasPrecision(5, 2);
        });

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Utilisateur");

            entity.HasIndex(e => e.IdGroupe, "IdGroupe");

            entity.Property(e => e.Mail).HasMaxLength(250);
            entity.Property(e => e.Mdp).HasMaxLength(300);
            entity.Property(e => e.Nom).HasMaxLength(200);
            entity.Property(e => e.Prenom).HasMaxLength(200);
            entity.Property(e => e.Telephone).HasMaxLength(20);

            entity.HasOne(d => d.IdGroupeNavigation).WithMany(p => p.Utilisateurs)
                .HasForeignKey(d => d.IdGroupe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Utilisateur_ibfk_1");
        });

        modelBuilder.Entity<Vehicule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Vehicule");

            entity.HasIndex(e => e.IdGroupe, "IdGroupe");

            entity.Property(e => e.Immatriculation).HasMaxLength(15);
            entity.Property(e => e.InfoComplementaire).HasMaxLength(1000);
            entity.Property(e => e.Nom).HasMaxLength(100);

            entity.HasOne(d => d.IdGroupeNavigation).WithMany(p => p.Vehicules)
                .HasForeignKey(d => d.IdGroupe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Vehicule_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
