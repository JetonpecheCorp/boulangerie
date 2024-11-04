import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { ButtonComponent } from '@component/button/button.component';
import { Recette } from '@model/Recette';
import { RecetteService } from '@service/Recette.service';
import { StopPropagationDirective } from '../../directive/stop-propagation.directive';
import { SelectionModel } from '@angular/cdk/collections';
import { MatCheckboxModule } from '@angular/material/checkbox';

@Component({
  selector: 'app-recette-produit',
  standalone: true,
  imports: [MatCheckboxModule, StopPropagationDirective, MatTableModule, MatTabsModule, MatDialogModule, MatButton, ButtonComponent],
  templateUrl: './recette-produit.component.html',
  styleUrl: './recette-produit.component.scss'
})
export class RecetteProduitComponent implements OnInit
{
  protected displayedColumns: string[] = ["checkbox", "nomIngredient", "quantite"];
  protected dataSource = signal<MatTableDataSource<Recette>>(new MatTableDataSource());
  protected selection = signal<SelectionModel<Recette>>(new SelectionModel<Recette>(true, []));

  private recetteServ = inject(RecetteService);
  private idPublicProduit: string = inject(MAT_DIALOG_DATA);

  ngOnInit(): void 
  {    
    this.Lister();
  }

  protected isAllSelected(): boolean
  {
    const numSelected = this.selection().selected.length;
    const numRows = this.dataSource().data.length;

    return numSelected === numRows;
  }

  protected toggleAllRows(): void
  {
    this.selection.update(x =>
    {
      if(this.isAllSelected())
        x.clear();

      else
        x.select(...this.dataSource().data);

      return x;
    });
  }

  protected checkboxLabel(_recette?: Recette): string 
  {
    if (!_recette)
      return `${this.isAllSelected() ? 'déselectionner' : 'selectionner'} tous`;

    return `${this.selection().isSelected(_recette) ? 'déselectionner' : 'selectionner'} ${_recette.nomIngredient}`;
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
