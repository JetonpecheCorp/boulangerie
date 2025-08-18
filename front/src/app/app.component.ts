import { BooleanInput } from '@angular/cdk/coercion';
import { BreakpointObserver } from '@angular/cdk/layout';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatDrawerMode, MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [MatListModule, RouterLink, RouterLinkActive, RouterOutlet, MatSidenavModule, MatToolbarModule, MatButtonModule, MatIconModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent 
{
  mdcBackdrop: BooleanInput = false;
  drawerMode: MatDrawerMode = "push";

  constructor(private breakpointObserver: BreakpointObserver) 
  {
    let breakpoint$ = this.breakpointObserver
      .observe([ '(max-width: 500px)']);

    breakpoint$.subscribe(() =>
      this.BreakpointChanges()
    );
  }

  protected EstConnecter(): boolean
  {
    return environment.utilisateur != undefined && environment.utilisateur != null;
  }

  private BreakpointChanges(): void 
  {
    if (this.breakpointObserver.isMatched('(max-width: 500px)')) 
    {
      this.drawerMode = "over";
      this.mdcBackdrop = true;
    } 
    else 
    {
      this.drawerMode = "push";
      this.mdcBackdrop = false;
    }
  }
}
