import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminService } from '../../services/admin.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-show-suppliers',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './show-suppliers.component.html'
})
export class ShowSuppliersComponent implements OnInit {
  suppliers: any[] = [];
  editingSupplierId: number | null = null;
  editableSupplier = { name: '', email: '', contact: '', password:'' };

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadSuppliers();
  }

  loadSuppliers() {
    this.adminService.getAllSuppliers().subscribe({
      next: (res) => {
        console.log(res);
        this.suppliers = res.data|| [];
      },
      error: () => {
        alert('Failed to load suppliers');
      }
    });
  }

  deleteSupplier(id: any) {
    if (confirm('Are you sure you want to delete this supplier?')) {
      this.adminService.deleteSupplier(id).subscribe({
        next: () => this.loadSuppliers(),
        error: () => alert('Failed to delete supplier')
      });
    }
  }

  startEdit(supplier: any) {
    console.log(supplier);
    this.editingSupplierId = supplier.id;
    this.editableSupplier = { ...supplier };
  }

  cancelEdit() {
    this.editingSupplierId = null;
  }

  saveUpdate(id: any) {
    this.adminService.updateSupplier(id, this.editableSupplier).subscribe({
      next: () => {
        this.editingSupplierId = null;
        this.loadSuppliers();
      },
      error: () => alert('Failed to update supplier')
    });
  }
}
