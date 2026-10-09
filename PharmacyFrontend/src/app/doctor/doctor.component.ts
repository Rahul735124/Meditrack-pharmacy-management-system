
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';
import { Router, RouterModule } from '@angular/router';
import { DoctorDashboardComponent } from './dashboard/doctor-dashboard.component';

@Component({
  selector: 'app-doctor',
  standalone: true,
  imports: [CommonModule, NavbarComponent, RouterModule,DoctorDashboardComponent],
  templateUrl: './doctor.component.html',
})
export class DoctorComponent {
  searchQuery: string = '';
  constructor(private router: Router) {}


  handleSearch(query: string): void {
    this.searchQuery = query;
  }
  isDashboardRoute(): boolean {
    return this.router.url === '/dashboard/doctor/dashboard';
  }



}

