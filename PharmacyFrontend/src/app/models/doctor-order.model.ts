export interface DoctorOrderStatus {

  orderId: number;

  orderDate: string;

  isVerified: boolean;

  isPickedUp: boolean;

  items: {

    drugId: number;

    drugName: string;

    quantity: number;

    price: number;

  }[];

}