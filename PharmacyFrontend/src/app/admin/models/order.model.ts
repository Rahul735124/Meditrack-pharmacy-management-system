export interface Order {
  orderId: number;
  userId: string;
  orderDate: string;
  isVerified: boolean;
  isPickedUp: boolean;

  orderItems:OrderItem[];
}

export interface OrderItem{
  orderItemId: number;
  drugId:number;
  drugName:string;
  quantity:number;
  isInStock: boolean;
  isRequestedFromSupplier?: boolean;
}

