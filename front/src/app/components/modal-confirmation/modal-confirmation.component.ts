import { Component, computed, inject, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';

type Message = 
{
  titre: string,
  message: string
}

@Component({
  selector: 'app-modal-confirmation',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule],
  templateUrl: './modal-confirmation.component.html',
  styleUrl: './modal-confirmation.component.scss'
})
export class ModalConfirmationComponent 
{
  info: Message = inject(MAT_DIALOG_DATA);

  titre = computed(() => this.info.titre);
  message = computed(() => this.info.message);
}
