import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DrugDoctor } from '../models/drug.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { ApiResponse } from './admin.service';
// import { DoctorOrder } from '../models/order.model';
import { DoctorOrderStatus } from '../models/doctor-order.model';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private apiUrl = 'https://meditrack-pharmacy-management-system.onrender.com/api/doctor';


  constructor(private http: HttpClient, private cookieService: CookieService) { }

  private getHeaders(): HttpHeaders {
    const token = this.cookieService.get('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getAllDrugs(): Observable<DrugDoctor[]> {
    return this.http.get<DrugDoctor[]>(`${this.apiUrl}/drugs`, { headers: this.getHeaders() });
  }

  placeOrder(orderPayload: { orderItems: { drugId: number; quantity: number }[] }): Observable<any> {
    return this.http.post(`${this.apiUrl}/place-order`, orderPayload, { headers: this.getHeaders() });
  }

  getDoctorOrders(): Observable<ApiResponse<DoctorOrderStatus[]>> {

    return this.http.get<ApiResponse<DoctorOrderStatus[]>>(`${this.apiUrl}/my-orders`, {

      headers: this.getHeaders()

    });

  }

  getProfile(): Observable<any> {
    return this.http.get(`${this.apiUrl}/profile`,{ headers: this.getHeaders() });
  }

  updateProfile(profileData: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/update-profile`, profileData,{ headers: this.getHeaders() });
  }



}
