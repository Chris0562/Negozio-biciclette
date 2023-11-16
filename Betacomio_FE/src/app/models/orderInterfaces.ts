import { CountedCart } from "./countedCart"

export interface OrderInfo {
    dueDate: Date,
    customerId: number,
    shipAddressId: number,
    billToAddressId: number,
    shipMethod: string,
    subTotal: number
    details: CountedCart[]
}

export interface OrderDetail {    
    orderQty: number,
    productId: number,
    unitPrice: number
}