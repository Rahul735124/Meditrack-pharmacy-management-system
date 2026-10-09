// src/app/doctor/dashboard/doctor-dashboard.component.ts
import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { DrugDoctor } from '../../models/drug.model';
import { DoctorService } from '../../services/doctor.service';
import { DrugCardComponent } from '../drug-card/drug-card.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [CommonModule, DrugCardComponent, RouterModule],
  templateUrl: './doctor-dashboard.component.html'
  //   styleUrls: ['./doctor-dashboard.component.css']
})
export class DoctorDashboardComponent implements OnInit, OnChanges {
  @Input() searchQuery: string = '';
  drugs: DrugDoctor[] = [];
  filteredDrugs: DrugDoctor[] = [];
  selectedDrugs: { drugId: number; name: string; quantity: number; price: any }[] = [];
  showSummary: boolean = false; // Controls visibility of order summary

  constructor(private doctorService: DoctorService) { }

  ngOnInit(): void {
    this.doctorService.getAllDrugs().subscribe({
      next: (res: any) => {
        this.drugs = res.data;
        this.filteredDrugs = [...this.drugs]; // Initialize filtered list
      },
      error: (err) => console.error(err)
    });
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['searchQuery']) {
      this.filterDrugs(); 
    }

  }
  filterDrugs(): void {
  const trimmedQuery = this.searchQuery.trim().toLowerCase();
  
  this.filteredDrugs = trimmedQuery
    ? this.drugs.filter(drug => drug.name.toLowerCase().includes(trimmedQuery))
    : [...this.drugs]; // ✅ Restore full list when search is empty
}



  // onSearch(query: string): void {
  //   if (!query.trim()) {
  //     this.filteredDrugs = this.drugs; // Show all drugs if search is empty
  //     return;
  //   }
  // }


  // Method to add drug with quantity
  addDrug(event: { drugId: number; quantity: number }) {
    const drug = this.drugs.find(d => d.id === event.drugId);
    if (!drug) return;

    const existingDrug = this.selectedDrugs.find(d => d.drugId === event.drugId);
    if (existingDrug) {
      existingDrug.quantity = event.quantity;
    } else {
      this.selectedDrugs.push({ drugId: event.drugId, name: drug.name, quantity: event.quantity, price: drug.price });
    }
  }

  // Toggle order summary when clicking cart icon
  toggleSummary() {
    this.showSummary = !this.showSummary;
  }

  // Confirm Order Method (Hits API)
  confirmOrder() {
    if (this.selectedDrugs.length === 0) {
      alert('No drugs selected!');
      return;
    }

    // Prepare request payload
    const orderPayload = {
      orderItems: this.selectedDrugs.map(drug => ({
        drugId: drug.drugId,
        quantity: drug.quantity
      }))
    };

    this.doctorService.placeOrder(orderPayload).subscribe({
      next: (res) => {
        console.log('Order placed successfully:', res);
        alert(`Order confirmed! ✅\nTotal Cost: ₹${res.data.totalCost}`);

        // Clear the selected drugs and hide summary after confirmation
        this.selectedDrugs = [];
        this.showSummary = false;
      },
      error: (err) => {
        console.error('Failed to place order:', err);
        alert('Error placing order. Please try again.');
      }
    });
  }

  getTotalCost(): number {
    return this.selectedDrugs.reduce((sum, drug) => sum + drug.quantity * drug.price, 0);
  }
}
