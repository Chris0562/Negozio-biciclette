export interface CustomerAddress {
  address: Address,  
  addressType: string
}

export interface Address {
  addressId: number,
  addressLine1: string,
  addressLine2: string,
  city: string,  
  countryRegion: string,
  stateProvince: string,
  postalCode: string
}


//interfaccia necessaria per postare in tabella customerAddress (addressID,customerID,addressType)
export interface PostCustomerAddress {
  addressID: number,
  customerID: number,
  addressType: string
}

//interfaccia "necessaria" per salvare risultato form in una variabile alla quale posso accedere
export interface AddressForm {
  addressLine1: string,
  addressLine2: string,
  city: string,  
  countryRegion: string,
  stateProvince: string,
  postalCode: string,
  addressType: string
}

export interface GetAddress {
  addressId: number,
  addressLine1: string,
  addressLine2: string,
  city: string,  
  countryRegion: string,
  stateProvince: string,
  postalCode: string,
  rowguid: string,
  modifiedDate: Date
}

export interface GetCustomerAddress {
  customerId: number,
  addressId: number,
  addressType: string,
  rowguid: string,
  modifiedDate: Date
}


