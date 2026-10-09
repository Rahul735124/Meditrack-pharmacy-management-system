import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminService } from '../../services/admin.service';
import { Order } from '../models/order.model';

@Component({
  selector: 'app-new-orders',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './new-orders.component.html',
})
export class NewOrdersComponent implements OnInit {
  orders: Order[] = [];
  loading = true;
  error = '';

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.fetchOrders();
  }

  fetchOrders(): void {
    this.loading = true;
    this.adminService.getNewOrders().subscribe({
      next: (res) => {
        // console.log("order:"+res);
        const orders = res; // <- fix: access the real data array
        console.log(orders);
        this.orders = Array.isArray(orders) ? orders.filter(o => !o.isVerified) : [];
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load orders';
        this.loading = false;
      }
    });
  }


  // approve(orderId: number): void {
  //   this.adminService.approveOrder(orderId).subscribe({
  //     next: () => {
  //       this.orders = this.orders.filter(o => o.orderId !== orderId);

  //     },
  //     error: (err) => {
  //       console.error('Order approval error:', err);
  //       this.error = `Failed to approve the order: ${err.message || 'Unknown error'}`;
  //     }

  //   });
  // }

  approve(orderId: number): void {
    this.adminService.approveOrder(orderId).subscribe({
      next: (response) => {
        console.log(response);
        if (response.success) {
          // Remove order only if successfully approved
          this.orders = this.orders.filter(order => order.orderId !== orderId);
        } else {
          // Show error message if stock is insufficient
          this.error = response.message;
        }
      },
      error: () => {
        this.error = 'Failed to approve the order due to an error';
      }
    });
  }

  requestFromSupplier(orderItemId: number, orderId:number): void {
    this.adminService.requestedOrderItemFromSupplier(orderItemId).subscribe({
      next: (res) => {
        console.log(res);
        const order = this.orders.find(o => o.orderId === orderId);
        const item = order?.orderItems.find(i => i.orderItemId === orderItemId);
        if (item) {
          item.isRequestedFromSupplier = true;
        }
        alert('Supplier has been requested for this item.');
        console.log(item?.isRequestedFromSupplier);
        this.fetchOrders();
      },
      error: () => {
        alert('Failed to request supplier.');
      }
    });
  }
}
