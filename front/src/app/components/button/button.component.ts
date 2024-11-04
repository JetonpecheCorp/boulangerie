import { Component, input, model, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [MatProgressSpinnerModule, MatButtonModule],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss'
})
export class ButtonComponent 
{
  label = input.required<string>();
  diametreSpinner = input<number>(30);
  classSpinner = input<string>("spinnerBtn");
  btnClicker = model.required<boolean>();

  onClick = output();

  protected Onclick()
  {
    this.onClick?.emit();
  }
}
