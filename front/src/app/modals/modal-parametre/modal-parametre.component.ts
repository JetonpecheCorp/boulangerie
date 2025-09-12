import { Component, inject, OnInit } from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ThemeService } from '@service/ThemeService.Service';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-modal-parametre',
  imports: [MatDialogModule, MatButtonModule, ReactiveFormsModule, MatInputModule, MatFormFieldModule],
  templateUrl: './modal-parametre.component.html',
  styleUrl: './modal-parametre.component.scss'
})
export class ModalParametreComponent implements OnInit
{
  protected form: FormGroup;

  private themeServ = inject(ThemeService);
  private toastrServ = inject(ToastrService);

  ngOnInit(): void 
  {
    this.form = new FormGroup({
      couleur: new FormControl("")
    });
  }

  protected Sauvegarder(): void
  {

  }

  protected AppercuTheme(ev: Event)
  {    
    this.themeServ.generateDynamicTheme(ev);
  }
}
