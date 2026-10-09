// import { Component } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { RouterModule } from '@angular/router';

// @Component({
//   standalone: true,
//   selector: 'app-admin',
//   imports: [CommonModule, RouterModule],
//   templateUrl: './admin.component.html',
// })
// export class AdminComponent {}
// src/app/admin/admin-dashboard.component.ts
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminService } from '../../services/admin.service';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
      <div class="cursor-pointer bg-white shadow-md rounded-lg p-6 hover:shadow-lg transition duration-300"
           [routerLink]="['/dashboard/admin/new-orders']">
        <h3 class="text-lg font-semibold text-gray-800 mb-2">New Orders</h3>
        <p class="text-3xl text-blue-600 font-bold">{{ newOrderCount }}</p>
        <p class="text-sm text-gray-500 mt-2">Click to view and approve</p>
      </div>
      <div class="cursor-pointer bg-green-100 hover:bg-green-200 text-green-900 p-6 rounded-xl shadow-md"
           [routerLink]="['/dashboard/admin/verified-orders']">
        <h3 class="text-xl font-semibold">Verified Orders</h3>
        <p class="text-sm">Click to view list of verified orders</p>
      </div>
      <div class="cursor-pointer bg-red-100 hover:bg-red-200 text-red-900 p-6 rounded-xl shadow-md"
     [routerLink]="['/dashboard/admin/expired-drugs']">
  <h3 class="text-xl font-semibold">Expired Drugs</h3>
  <p class="text-3xl font-bold text-red-600">{{ expiredDrugCount }}</p>
  <p class="text-sm">Click to view expired drugs</p>
</div>

    </div>
  `
})
export class AdminDashboardComponent implements OnInit {
  newOrderCount = 0;
  expiredDrugCount = 0;
  constructor(private adminService: AdminService, private cdr: ChangeDetectorRef) { }
  ngOnInit(): void {
    this.adminService.getNewOrders().subscribe({
      next: res => {
        var orders = res
        console.log(orders);
        this.newOrderCount = Array.isArray(orders)
          ? orders.filter(o => !o.isVerified).length
          : 0;
        console.log(this.newOrderCount);
        this.cdr.detectChanges();
      },
      error: () => {
        this.newOrderCount = 0;
        this.cdr.detectChanges();
      }
    });

    this.adminService.getExpiredDrugs().subscribe({
      next: res => {
        this.expiredDrugCount = Array.isArray(res.data) ? res.data.length : 0;
        console.log(res.data);
        console.log("expired: "+this.expiredDrugCount);
        this.cdr.detectChanges();
      },
      error: () => {
        this.expiredDrugCount = 0;
        this.cdr.detectChanges();
      }
    });
  }
}

  


