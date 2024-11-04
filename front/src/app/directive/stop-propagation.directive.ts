import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[stop-propagation]',
  standalone: true
})
export class StopPropagationDirective 
{

  @HostListener("click", ["$event"])
  public StopPropagationClick(event: Event)
  {
    event.stopPropagation();
  }
}
