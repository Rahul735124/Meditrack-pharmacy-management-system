import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminService } from '../../services/admin.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-show-drugs',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './show-drugs.component.html',
})
export class ShowDrugsComponent implements OnInit {
  drugs: any[] = [];
  selectedDrug: any = null;
  successMessage = '';
  errorMessage = '';

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadDrugs();
  }

  loadDrugs(): void {
    this.adminService.getAllDrugs().subscribe({
      next: (res) => {
        console.log(res);
        this.drugs = res.data;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load drugs';
      }
    });
  }

  deleteDrug(id: any): void {
    this.adminService.deleteDrug(id).subscribe({
      next: () => {
        this.successMessage = 'Drug deleted successfully!';
        this.loadDrugs();
      },
      error: () => {
        this.errorMessage = 'Failed to delete drug';
      }
    });
  }

  editDrug(drug: any): void {
    this.selectedDrug = { ...drug }; // create a copy
  }

  updateDrug(): void {
    this.adminService.updateDrug(this.selectedDrug.id, this.selectedDrug).subscribe({
      next: () => {
        this.successMessage = 'Drug updated successfully!';
        this.selectedDrug = null;
        this.loadDrugs();
      },
      error: () => {
        this.errorMessage = 'Failed to update drug';
      }
    });
  }

  cancelEdit(): void {
    this.selectedDrug = null;
  }
}
