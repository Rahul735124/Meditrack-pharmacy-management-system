import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SupplierService {

  private apiUrl = 'http://localhost:5118/api/supplier';

  constructor(private http: HttpClient, private cookieService: CookieService) { }

  private getHeaders(): HttpHeaders {
    const token = this.cookieService.get('token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  getNewOrders(): Observable<any> {
    return this.http.get<{ success: boolean; data: any; message: string }>(
      `${this.apiUrl}/requests`, { headers: this.getHeaders() }
    );
  }

  confirmAndSendOrder(orderItemId: number): Observable<any> {
    return this.http.post<{ success: boolean; data: any; message: string }>(
      `${this.apiUrl}/confirm-and-send/${orderItemId}`,
      {},
      { headers: this.getHeaders() }
    );
  }


}
