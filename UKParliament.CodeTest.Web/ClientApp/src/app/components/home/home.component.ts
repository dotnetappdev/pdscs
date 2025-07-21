import { Component } from '@angular/core';
import { Router } from '@angular/router';   // Import Router

import { PersonService } from '../../services/person.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  constructor(private router: Router, private personService: PersonService) { 
    this.getPersonById(1);
  }

  getPersonById(id: number): void {
    this.personService.getById(id).subscribe({
      next: (result) => console.info(`User returned: ${JSON.stringify(result)}`),
      error: (e) => console.error(`Error: ${e}`)
    });
  }

  goToPersons(): void {
    this.router.navigate(['/persons']);
  }
}
