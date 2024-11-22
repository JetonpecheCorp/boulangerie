import { Component, input, model, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { StopPropagationDirective } from '../../directive/stop-propagation.directive';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [StopPropagationDirective, MatIconModule, MatProgressSpinnerModule, MatButtonModule],
  templateUrl: './button.component.html',
  styleUrl: './button.component.scss',
})
export class ButtonComponent 
{
  label = input<string>("");
  diametreSpinner = input<number>(30);
  classSpinner = input<string>("spinnerBtn");
  btnClicker = model<boolean>(false);
  icon = input<string | null>(null);
  style = input<"flat" | "raised" | "miniFab" | "basic">("flat");

  onClick = output();

  protected Onclick()
  {
    this.onClick?.emit();
  }
}
