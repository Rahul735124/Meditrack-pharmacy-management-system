import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { SupplierOrderStatus } from '../models/supplier-order-status.model';
import { CommonModule, NgClass } from '@angular/common';

@Component({
  selector: 'app-supplier-order-status',
  standalone: true,
  imports: [NgClass, CommonModule],
  templateUrl: './supplier-order-status.component.html',
})
export class SupplierOrderStatusComponent implements OnInit {
  supplierOrders: SupplierOrderStatus[] = [];
  loading = false;
  error: string | null = null;

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.fetchSupplierOrders();
  }

  fetchSupplierOrders(): void {
    this.loading = true;
    this.adminService.getSupplierOrderStatuses().subscribe({
      next: (data) => {
        this.supplierOrders = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load supplier order statuses.';
        this.loading = false;
      },
    });
  }
}
