import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { AdminComponent } from './admin/admin.component';
import { AddDrugComponent } from './admin/add-drug/add-drug.component';
import { ShowDrugsComponent } from './admin/show-drugs/show-drugs.component';
import { AddSupplierComponent } from './admin/add-supplier/add-supplier.component';
import { ShowSuppliersComponent } from './admin/show-suppliers/show-suppliers.component';
import { AdminDashboardComponent } from './admin/dashboard/dashboard.component';
import { VerifiedOrdersComponent } from './admin/verified-orders/verified-orders.component';
import { NewOrdersComponent } from './admin/new-orders/new-orders.component';
import { DoctorDashboardComponent } from './doctor/dashboard/doctor-dashboard.component';
import { SupplierOrderStatusComponent } from './admin/supplier-order-status/supplier-order-status.component';
import { DoctorOrdersComponent } from './doctor/order-summary/doctor-order.component';
import { DoctorComponent } from './doctor/doctor.component';
import { SupplierComponent } from './supplier/supplier.component';
import { SupplierDashboardComponent } from './supplier/supplier-dashboard/supplier-dashboard.component';
import { OrderRequestComponent } from './supplier/order-request/order-request.component';
import { SalesReportComponent } from './admin/sales-report/sales-report.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { DoctorProfileComponent } from './doctor/doctor-profile/doctor-profile.component';
import { ExpiredDrugsComponent } from './admin/expired-drugs/expired-drugs.component';
// import { DoctorOrdersComponent } from './doctor/doctor-order/doctor-order.component';
// import { OrderSummaryComponent } from './doctor/order-summary/order-summary.component';

export const appRoutes: Routes = [
  {
    path: 'auth',
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'forgot-password', component: ForgotPasswordComponent },
      { path: 'reset-password', component: ResetPasswordComponent },

      
    ],
  },
  {
    path: 'dashboard/admin',
    component: AdminComponent,
    children: [
      // { path: '', component: AdminComponent }, // Default child route (sidebar + home)
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: AdminDashboardComponent },
      { path: 'add-drug', component: AddDrugComponent },
      { path: 'show-drugs', component: ShowDrugsComponent },
      { path: 'add-supplier', component: AddSupplierComponent },
      { path: 'show-suppliers', component: ShowSuppliersComponent },
      { path: 'verified-orders', component: VerifiedOrdersComponent },
      { path: 'new-orders', component: NewOrdersComponent },
      { path: 'supplier-order-status', component: SupplierOrderStatusComponent },
      { path: 'sales-report', component: SalesReportComponent },
      { path: 'expired-drugs', component: ExpiredDrugsComponent }






    ],
  },
  {
    path: 'dashboard/doctor',
    component: DoctorComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }, 
    { path: 'dashboard', component: DoctorDashboardComponent }, 
    { path: 'orders', component: DoctorOrdersComponent },
    { path: 'profile', component: DoctorProfileComponent}


    ]
  },
  {
    path: 'dashboard/supplier',
    component: SupplierComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },

      { path: 'dashboard', component: SupplierDashboardComponent },
      { path: 'order-request', component: OrderRequestComponent },



    ]
  },

  // { path: 'dashboard/doctor', component: DoctorDashboardComponent },
  //     { path: 'doctor/orders', component: DoctorOrdersComponent }, 

  // { path: 'dashboard/supplier', component: SupplierComponent },
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
];
