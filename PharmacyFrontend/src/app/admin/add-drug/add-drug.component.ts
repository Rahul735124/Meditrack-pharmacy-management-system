import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-add-drug',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-drug.component.html',
})
export class AddDrugComponent implements OnInit {
  drug = {
    name: '',
    description: '',
    price: 0,
    quantity: 0,
    expiryDate: '',
    supplierId: ''
  };

  suppliers: any[] = [];


  successMessage = '';
  errorMessage = '';

  constructor(private adminService: AdminService) { }
  ngOnInit(): void {
    this.loadSuppliers();
  }

  loadSuppliers() {
    this.adminService.getAllSuppliers().subscribe({
      next: (res) => this.suppliers = res.data || [],
      error: () => alert('Failed to load suppliers')
    });
  }



  onSubmit(): void {
    this.adminService.addDrug(this.drug).subscribe({
      next: (res) => {
        this.successMessage = res.message;
        this.errorMessage = '';
        this.drug = { name: '', description: '', price: 0, quantity: 0, expiryDate: '', supplierId: '' };
      },
      error: (err) => {
        console.error(err);
        this.successMessage = '';
        if (err.status === 404) {
          this.errorMessage = 'Supplier not found!';
        } else if (err.status === 400) {
          this.errorMessage = 'Invalid data! Please check your inputs.';
        } else {
          this.errorMessage = 'Failed to add drug. Please try again later.';
        }

      }
    });
  }
}
