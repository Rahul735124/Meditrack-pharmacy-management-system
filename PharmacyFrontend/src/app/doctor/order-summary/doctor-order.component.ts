import { Component, OnInit } from '@angular/core';

import { CommonModule } from '@angular/common';

import { DoctorService } from '../../services/doctor.service';

import { DoctorOrderStatus } from '../../models/doctor-order.model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({

  selector: 'app-doctor-orders',

  standalone: true,

  imports: [CommonModule],

  templateUrl: './doctor-order.component.html',

})

export class DoctorOrdersComponent implements OnInit {

  orders: DoctorOrderStatus[] = [];

  loading = false;

  error: string | null = null;

  constructor(private doctorService: DoctorService, private router:Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    console.log('Current Route:', this.route.snapshot.url);


    this.fetchOrders();

  }

  fetchOrders() {

    this.loading = true;
    console.log('Fetching ');

    this.doctorService.getDoctorOrders().subscribe({

      next: (res) => {
        console.log(res);

        this.orders = res.data || [];
        console.log(this.orders);

        this.loading = false;

      },

      error: () => {

        this.error = 'Failed to load your orders';

        this.loading = false;

      },

    });

  }
  getOrderTotal(order: DoctorOrderStatus): number {
    if(!order || !order.items){
      return 0;
    }
    return order.items.reduce((total, item) => total + (item.price * item.quantity), 0);
  }

  
  closeOrdersView() {
    this.router.navigate(['/dashboard/doctor']); 
  }

  }