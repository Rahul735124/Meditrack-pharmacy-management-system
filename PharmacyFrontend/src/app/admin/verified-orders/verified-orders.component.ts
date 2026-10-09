import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminService } from '../../services/admin.service';
import { Order } from '../models/order.model';
// import { Order } from '../../models/order.model';


@Component({
  selector: 'app-verified-orders',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './verified-orders.component.html',
})
export class VerifiedOrdersComponent implements OnInit {
  verifiedOrders: Order[] = [];
  loading = false;
  error: string | null = null;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.fetchVerifiedOrders();
  }

  fetchVerifiedOrders() {
    this.loading = true;
    this.adminService.getVerifiedOrders().subscribe({
      next: (res) => {
        this.verifiedOrders = res.data as Order[];
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load verified orders';
        this.loading = false;
      },
    });
  }

  pickUpOrder(orderId: number): void {
    if (confirm('Are you sure you want to mark this order as picked up?')) {
      this.adminService.pickUpOrder(orderId).subscribe({
        next: (res) => {
          alert(res.data || 'Order picked successfully.');
          this.fetchVerifiedOrders();  // Reload data
        },
        error: () => {
          alert('Failed to pick up order.');
        }
      });
    }
  }

}
