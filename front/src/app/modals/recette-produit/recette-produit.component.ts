import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { ButtonComponent } from '@component/button/button.component';
import { Recette } from '@model/Recette';
import { RecetteService } from '@service/Recette.service';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ThemeService } from '@service/ThemeService.Service';
import { MatIconModule } from '@angular/material/icon';
import { AjouterModifierRecetteComponent } from "../ajouter-modifier-recette/ajouter-modifier-recette.component";

@Component({
  selector: 'app-recette-produit',
  standalone: true,
  imports: [MatDialogModule, MatIconModule, MatCheckboxModule, MatTableModule, MatTabsModule, MatButton, ButtonComponent, AjouterModifierRecetteComponent],
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
  private matDialog = inject(MatDialog);

  private infos = inject(MAT_DIALOG_DATA);
  
  ngOnInit(): void 
  {    
    this.Lister();
  }

  protected OuvrirModal(_recette?: Recette): void
  {
    let listeIdPublicIngredient: string[] = [];

    if(!_recette)
    {
      for (const element of this.dataSource().data) 
        listeIdPublicIngredient.push(element.idPublicIngredient);
    }

    const DIALOG_REF = this.matDialog.open(AjouterModifierRecetteComponent, { 
      data: {
        idPublicProduit: this.infos.idPublicProduit,
        nomProduit: this.infos.nomProduit,
        recette: _recette,
        listeIdPublicIngredient: listeIdPublicIngredient
      },
      disableClose: true
    });

    DIALOG_REF.afterClosed().subscribe({
      next: (retour: Recette[] | number) =>
      {
        if(!retour)
          return;

        if(_recette)
        {
          _recette.quantite = retour as number;
        }
        else
        {
          this.dataSource.update(x =>
          {
            for (const element of retour as Recette[])
              x.data.push(element);
  
            return x;
          });
        }

        this.dataSource().data = this.dataSource().data;
      }
    });
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
    });
  }

  private Lister(): void
  {
    this.recetteServ.Lister(this.infos.idPublicProduit).subscribe({
      next: (retour) =>
      {        
        this.dataSource.update(x =>
        {
          x.data = retour;
          return x;
        });
      }
    });
  }
}
