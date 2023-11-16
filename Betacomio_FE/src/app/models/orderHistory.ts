export interface OrderHistory {
    salesOrderId: number,
    orderDate: Date,
    dueDate: Date,
    status: number,
    salesOrderNumber: string,
    customerId: number,
    totalDue: number,
    addressLine1: string,
    addressLine2: string,
    city: string,
    stateProvince: string,
    countryRegion: string,
    postalCode: string,
    details: Details[]
  }

  export interface Details {
    salesOrderId: number,
    salesOrderDetailId: number,
    orderQty: number,
    productId: number,
    lineTotal: number
  }