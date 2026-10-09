import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { Order } from '../admin/models/order.model';
import { SupplierOrderStatus } from '../admin/models/supplier-order-status.model';

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = 'http://localhost:5118/api/admin';

  constructor(private http: HttpClient, private cookieService: CookieService) { }

  private getHeaders(): HttpHeaders {
    const token = this.cookieService.get('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  // Get all drugs
  getAllDrugs(): Observable<any> {
    return this.http.get(`${this.apiUrl}/drugs`, { headers: this.getHeaders() });
  }

  addSupplier(supplier: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/add-supplier`, supplier, { headers: this.getHeaders() });
  }

  getAllSuppliers(): Observable<any> {
    return this.http.get(`${this.apiUrl}/suppliers`, { headers: this.getHeaders() });
  }


  addDrug(drug: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/add-drug`, drug, { headers: this.getHeaders() });
  }

  updateDrug(id: string, drug: any) {
    return this.http.put(`${this.apiUrl}/edit-drug/${id}`, drug, { headers: this.getHeaders() });
  }

  deleteDrug(id: string) {
    return this.http.delete(`${this.apiUrl}/delete-drug/${id}`, { headers: this.getHeaders() });
  }

  updateSupplier(id: string, supplierData: any) {
    return this.http.put(`${this.apiUrl}/edit-supplier/${id}`, supplierData, { headers: this.getHeaders() });
  }

  deleteSupplier(id: string) {
    return this.http.delete(`${this.apiUrl}/delete-supplier/${id}`, { headers: this.getHeaders() });
  }

  getNewOrders(): Observable<Order[]> {
    return this.http.get<{ success: boolean; data: Order[]; message: string }>(
      `${this.apiUrl}/orders/new`,
      { headers: this.getHeaders() }
    ).pipe(
      map((res: { success: boolean; data: Order[]; message: string }) => res.data)
    );
  }

  approveOrder(orderId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/orders/verify/${orderId}`, {}, { headers: this.getHeaders() });
  }

  getVerifiedOrders(): Observable<{ success: boolean; data: Order[]; message: string }> {
    return this.http.get<{ success: boolean; data: Order[]; message: string }>(
      `${this.apiUrl}/orders/verified`, { headers: this.getHeaders() }
    );
  }

  requestedOrderItemFromSupplier(orderItemId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/orders/classify/${orderItemId}`, {}, { headers: this.getHeaders() });
  }

  getSupplierOrderStatuses(): Observable<SupplierOrderStatus[]> {
    return this.http.get<any>(`${this.apiUrl}/supplier-order-status`, { headers: this.getHeaders() }).pipe(
      map(response => response.data as SupplierOrderStatus[])
    );
  }

  pickUpOrder(orderId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/orders/pick/${orderId}`, null, { headers: this.getHeaders() });
  }

  getSalesReport(startDate: string, endDate: string): Observable<any> {
    return this.http.get<{ success: boolean; data: any[]; message: string }>(
      `${this.apiUrl}/sales-report?startDate=${startDate}&endDate=${endDate}`,
      { headers: this.getHeaders() }
    );
  }

  getExpiredDrugs(): Observable<{ success: boolean; message: string; data: any[] }> {
    return this.http.get<{ success: boolean; message: string; data: any[] }>(`${this.apiUrl}/expired-drugs`, { headers: this.getHeaders() });
  }







}
