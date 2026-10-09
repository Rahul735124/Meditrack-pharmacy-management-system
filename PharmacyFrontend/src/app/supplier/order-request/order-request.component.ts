import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SupplierService } from '../../services/supplier.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-orders-request',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-request.component.html',
})
export class OrderRequestComponent implements OnInit {
    orders: any[]=[];
  

  constructor(private supplierService: SupplierService, private router:Router) { }

  ngOnInit(): void {
    this.fetchOrders();
  }

  fetchOrders(): void {
    
    this.supplierService.getNewOrders().subscribe({
      next: (res) => {
        this.orders=res.data || [];
        console.log("order:"+this.orders);
    
      }
    });
  }

  confirmAndSend(orderItemId: number): void {
    this.supplierService.confirmAndSendOrder(orderItemId).subscribe({
      next: () => {
        this.fetchOrders(); // Refresh list
      },
      error: (err) => {
        console.error('Error confirming order:', err);
      },
    });
  }

  exitOrderRequest(): void {
    this.router.navigate(['/dashboard/supplier']);
  }





}
