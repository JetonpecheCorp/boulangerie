import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { ButtonComponent } from '@component/button/button.component';
import { Recette } from '@model/Recette';
import { RecetteService } from '@service/Recette.service';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ThemeService } from '@service/ThemeService.Service';

@Component({
  selector: 'app-recette-produit',
  standalone: true,
  imports: [MatCheckboxModule, MatTableModule, MatTabsModule, MatDialogModule, MatButton, ButtonComponent],
  templateUrl: './recette-produit.component.html',
  styleUrl: './recette-produit.component.scss'
})
export class RecetteProduitComponent implements OnInit
{
  protected displayedColumns: string[] = ["nomIngredient", "quantite", "action"];
  protected dataSource = signal<MatTableDataSource<Recette>>(new MatTableDataSource());
  protected btnClicker = signal(false);

  private recetteServ = inject(RecetteService);
  private themeServ = inject(ThemeService);
  private idPublicProduit: string = inject(MAT_DIALOG_DATA);

  ngOnInit(): void 
  {    
    this.Lister();
  }

  protected OuvrirModalConfirmation(_recette: Recette): void
  {
    const TITRE = "Confirmation supression";
    const MSG = `Confirmez vous la suppression de l'ingredient '${_recette.nomIngredient}' du produit '${_recette.nomProduit}'`

    this.themeServ.OuvrirConfirmation(TITRE, MSG);
    this.themeServ.retourConfirmation.subscribe({
      next: (retour: boolean) =>
      {
        if(retour)
          this.Supprimer(_recette.idPublicProduit, _recette.idPublicIngredient);
      }
    });
  }

  private Supprimer(_idPublicProduit: string, _idPublicIngredient: string): void
  {
    this.btnClicker.set(true);

    this.recetteServ.Supprimer(_idPublicProduit, _idPublicIngredient).subscribe({
      next: () => 
      {
        this.dataSource.update(x => 
        {
          x.data = x.data.filter(y => y.idPublicIngredient != _idPublicIngredient);
          return x;
        });

        this.btnClicker.set(false);
      }
    })
    
  }

  private Lister(): void
  {
    this.recetteServ.Lister(this.idPublicProduit).subscribe({
      next: (retour) =>
      {
        console.log(retour);
        
        this.dataSource.update(x =>
        {
          x.data = retour;
          return x;
        });
      }
    });
  }
}
