import { Product } from "./product";

export interface CountedCart {
  product: Product,
  orderQty: number
}