import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-add-supplier',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-supplier.component.html'
})
export class AddSupplierComponent {
  supplier = {
    name: '',
    email: '',
    contact: '',
    password: ''
  };

  successMessage = '';
  errorMessage = '';

  constructor(private adminService: AdminService) { }

  addSupplier() {
    this.adminService.addSupplier(this.supplier).subscribe({
      next: (res) => {
        this.errorMessage = '';
        this.supplier = { name: '', email: '', contact: '', password: '' };
        this.successMessage = res.message; // Shows success message from backend
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = err.error?.message || 'Failed to add supplier.'; // Displays detailed backend error
        this.successMessage = '';
      }
    });
  }
}
