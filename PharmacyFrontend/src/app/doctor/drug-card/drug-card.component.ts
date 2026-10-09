// src/app/doctor/drug-card/drug-card.component.ts
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DrugDoctor } from '../../models/drug.model';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-drug-card',
  standalone: true,
  imports: [CommonModule,RouterModule,FormsModule],
  templateUrl: './drug-card.component.html'
  //   styleUrls: ['./drug-card.component.css']
})
export class DrugCardComponent {
  @Input() drug!: DrugDoctor;
  @Output() addDrug = new EventEmitter<{ drugId: number; quantity: number }>();

  quantity: number = 1;


  addToCart() {
  if (isNaN(this.quantity) || this.quantity <= 0) {
    alert('Please enter a valid quantity greater than zero.');
    return;
  }
  
  this.addDrug.emit({ drugId: this.drug.id, quantity: this.quantity });
}
incrementQuantity(): void {
  this.quantity++;
}

decrementQuantity(): void {
  if (this.quantity > 1) {
    this.quantity--;
  }
}


}
