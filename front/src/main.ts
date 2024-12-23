import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { NomJourSemaine } from '@model/calendrier/NomJourSemaine';

declare global {
  interface Date {
    nbJourDiff(_date: Date): number;
    datePremierJourSemaine(): Date;
    nomJour(): string;
    ajouterJour(_nbJour: number): Date;
    ajouterMois(_nbMois: number): Date;
    setDebutMois(): Date;
    setFinMois(): Date;
    debutMois(): Date;
    finMois(): Date;
    toISOFormat(): string;
  }

  interface DateConstructor {
    listerNomJourSemaine(): NomJourSemaine[]
  }
}

Date.listerNomJourSemaine = function (): NomJourSemaine[]
{
  return [{ 
      nom: "Lundi",
      abreviation: "Lun.",
      lettre: "L"
    }, 
    {
      nom: "Mardi",
      abreviation: "Mar.",
      lettre: "M"
    }, 
    {
      nom: "Mercredi",
      abreviation: "Mer.",
      lettre: "Me"
    }, 
    {
      nom: "Jeudi",
      abreviation: "Jeu.",
      lettre: "J"
    }, 
    {
      nom: "Vendredi",
      abreviation: "Ven.",
      lettre: "V"
    }, 
    {
      nom: "Samedi",
      abreviation: "Sam.",
      lettre: "S"
    }, 
    {
      nom: "Dimanche",
      abreviation: "Dim.",
      lettre: "D"
    }];
};

Date.prototype.toISOFormat = function (): string
{
  return `${this.getFullYear()}-${this.getMonth() + 1}-${this.getDate()}`;
}

Date.prototype.setDebutMois = function (): Date
{
  this.setDate(1);

  return this;
} 

Date.prototype.setFinMois = function (): Date
{
  this.ajouterMois(1);
  this.setDate(0);
  
  return this;
} 

Date.prototype.debutMois = function (): Date
{  
  return new Date(this.getFullYear(), this.getMonth(), 1);
} 

Date.prototype.finMois = function (): Date
{
  return new Date(this.getFullYear(), this.getMonth() + 1, 0);
} 

Date.prototype.ajouterMois = function (_nbMois: number): Date 
{
  this.setMonth(this.getMonth() + _nbMois);

  return this;
}

Date.prototype.ajouterJour = function (_nbJour: number): Date 
{
  this.setDate(this.getDate() + _nbJour);

  return this;
};

Date.prototype.nomJour = function (): string 
{
  const INDEX = this.getDay() == 0 ? 6 : this.getDay() - 1;

  return Date.listerNomJourSemaine()[INDEX].nom;
};

Date.prototype.nbJourDiff = function (_date: Date): number 
{  
  const DIFF_TEMPS = Math.abs(_date.getTime() - this.getTime());
  const DIFF_JOUR = Math.floor(DIFF_TEMPS / (1000 * 60 * 60 * 24)); 

  return DIFF_JOUR;
};

Date.prototype.datePremierJourSemaine = function(): Date
{
  let day = this.getDay();
  let diff = this.getDate() - day + (day == 0 ? -6 : 1); 

  let date = new Date(this);
  date.setDate(diff)

  return date;
};

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
