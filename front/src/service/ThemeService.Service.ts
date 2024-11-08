import { DestroyRef, inject, output } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatDialog } from '@angular/material/dialog';
import { ModalConfirmationComponent } from '@component/modal-confirmation/modal-confirmation.component';
import { argbFromHex, themeFromSourceColor, applyTheme } from '@material/material-color-utilities';
import { Subject } from 'rxjs';

export class ThemeService
{
  private matDialog = inject(MatDialog);
  private destroyRef = inject(DestroyRef);
  retourConfirmation: Subject<boolean>;

  OuvrirConfirmation(_titre: string, _message: string): void
  {
    this.retourConfirmation = new Subject<boolean>();
    
    const DIALOG_REF = this.matDialog.open(ModalConfirmationComponent, { data: { 
        titre: _titre,
        message: _message
      } 
    });

    DIALOG_REF.afterClosed().pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (retour: boolean) =>
      {
        this.retourConfirmation.next(retour === true);
        this.retourConfirmation.complete();
      }
    });
  }

    generateDynamicTheme(ev: Event): void
    {
        const fallbackColor = '#3040b6';
        const sourceColor = (ev.target as HTMLInputElement).value;
    
        let argb;
        
        try 
        {
          argb = argbFromHex(sourceColor);
        } 
        catch (error) 
        {
          // falling to default color if it's invalid color
          argb = argbFromHex(fallbackColor);
        }
    
        const targetElement = document.documentElement;
    
        // Get the theme from a hex color
        const theme = themeFromSourceColor(argb);
    
        // Print out the theme as JSON
        console.log(JSON.stringify(theme, null, 2));
    
        // Identify if user prefers dark theme
        const systemDark = window.matchMedia(
          '(prefers-color-scheme: dark)'
        ).matches;
    
        // Apply theme to root element
        applyTheme(theme, {
          target: targetElement,
          dark: false,
          brightnessSuffix: true,
        });
    
        const styles = targetElement.style;
    
        for (const key in styles) {
          if (Object.prototype.hasOwnProperty.call(styles, key)) {
            const propName = styles[key];
            if (propName.indexOf('--md-sys') === 0) {
              const sysPropName = '--sys' + propName.replace('--md-sys-color', '');
              targetElement.style.setProperty(
                sysPropName,
                targetElement.style.getPropertyValue(propName)
              );
            }
          }
        }
      }
}