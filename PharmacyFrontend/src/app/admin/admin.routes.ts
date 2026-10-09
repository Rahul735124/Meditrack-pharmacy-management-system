import { Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { AddDrugComponent } from './add-drug/add-drug.component';
import { AddSupplierComponent } from './add-supplier/add-supplier.component';
import { ShowDrugsComponent } from './show-drugs/show-drugs.component';
import { ShowSuppliersComponent } from './show-suppliers/show-suppliers.component';
import { NewOrdersComponent } from './new-orders/new-orders.component';
import { VerifiedOrdersComponent } from './verified-orders/verified-orders.component';
import { AdminDashboardComponent } from './dashboard/dashboard.component';
import { SupplierOrderStatusComponent } from './supplier-order-status/supplier-order-status.component';
import { SalesReportComponent } from './sales-report/sales-report.component';

export const adminRoutes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      // { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: '', component: AdminDashboardComponent },
      // { path: 'dashboard', component: AdminDashboardComponent },
      { path: 'add-drug', component: AddDrugComponent },
      { path: 'show-drugs', component: ShowDrugsComponent },
      { path: 'add-supplier', component: AddSupplierComponent },
      { path: 'show-suppliers', component: ShowSuppliersComponent },
      { path: 'new-orders', component: NewOrdersComponent },
      { path: 'verified-orders', component: VerifiedOrdersComponent },
      { path: 'supplier-order-status', component: SupplierOrderStatusComponent },
      { path: 'sales-report', component: SalesReportComponent }


    ]
  }
];
