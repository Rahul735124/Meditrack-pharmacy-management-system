import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from './sidebar/sidebar.component';
import { NavbarComponent } from '../navbar/navbar.component';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, SidebarComponent, NavbarComponent, RouterModule],
  templateUrl: './admin.component.html',
})
export class AdminComponent {
  showMobileSidebar=false;
  toggleMobileSidebar() {
    this.showMobileSidebar = !this.showMobileSidebar;
  }

  closeMobileSidebar() {
    this.showMobileSidebar = false;
  }
}

