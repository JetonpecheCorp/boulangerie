import { Component, computed, input, model, OnInit, signal, ɵINPUT_SIGNAL_BRAND_WRITE_TYPE } from '@angular/core';
import { AbstractControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [MatFormFieldModule, MatButtonModule, MatIconModule, MatInputModule, ReactiveFormsModule],
  templateUrl: './input.component.html',
  styleUrl: './input.component.scss'
})
export class InputComponent implements OnInit
{
  type = model<String>("text");
  maxLength = input<number>(-1);
  placeholder = input<String>("");

  label = input.required<String>();
  controle = input.required<AbstractControl>();

  protected mdpVisible = signal(false);
  protected estInputMdp = false;

  ngOnInit(): void 
  {
    this.estInputMdp = this.type() == "password";
  }

  protected ModifierVisibiliteMdp(): void
  {
    this.mdpVisible.set(!this.mdpVisible());
    this.type.update((x) => x == "text" ? "password" : "text");
  }
}
