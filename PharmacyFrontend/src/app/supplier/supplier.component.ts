
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-supplier',
  standalone: true,
  imports: [CommonModule, NavbarComponent, RouterModule],
  templateUrl: './supplier.component.html',
})
export class SupplierComponent {}

