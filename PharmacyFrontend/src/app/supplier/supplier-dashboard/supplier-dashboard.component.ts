// src/app/doctor/dashboard/doctor-dashboard.component.ts
import { Component, OnInit } from '@angular/core';


import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../navbar/navbar.component';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './supplier-dashboard.component.html'
  //   styleUrls: ['./doctor-dashboard.component.css']
})
export class SupplierDashboardComponent {
  
}
