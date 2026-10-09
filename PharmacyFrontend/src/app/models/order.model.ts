export interface OrderItem {
  drugId: number;
  drugName: string;
  quantity: number;
}

export interface Order {
  orderId: number;
  userId: string;
  orderDate: string;
  isVerified: boolean;
  orderItems: OrderItem[];
}

export interface DoctorOrder {

  orderId: number;

  orderDate: string;

  totalItems: number;

  totalQuantity: number;

  isVerified: boolean;

  isPickedUp: boolean;

}