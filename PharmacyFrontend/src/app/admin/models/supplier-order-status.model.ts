export interface SupplierOrderStatus {
  orderItemId: number;
  drugName: string;
  quantity: number;
  orderDate: string;
  status: 'Confirmed' | 'Requested';
}
