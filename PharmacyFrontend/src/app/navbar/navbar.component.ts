import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './navbar.component.html'
})


export class NavbarComponent {

  

  @Input() role: string = '';
  @Output() searchEvent = new EventEmitter<string>();
  dropdownOpen = false;
  searchQuery: string = '';

  constructor(
    private cookieService: CookieService,
    private router: Router
  ) {}

  toggleDropdown(): void {
    this.dropdownOpen = !this.dropdownOpen;
  }

  closeDropdown(): void {
    setTimeout(() => this.dropdownOpen = false, 150); // slight delay to allow click
  }
  
  logout(): void {
    this.cookieService.delete('token'); // Delete JWT cookie
    this.router.navigate(['auth/login']);       // Redirect to login
  }
  onSearch(): void {
    this.searchEvent.emit(this.searchQuery.trim()); // Emit search query
  }

  goToProfile(){
    this.router.navigate(['/doctor/profile'])

  }

}
