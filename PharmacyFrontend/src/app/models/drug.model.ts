export interface Drug {
    id: number;
    name: string;
    description: string;
    price: number;
    quantityAvailable: number;
    expiryDate: string;
    supplierName: string;

  }


export interface DrugDoctor {
  id: number;
  name: string;
  description: string;
  price: number;
  expiryDate: string;
}

  