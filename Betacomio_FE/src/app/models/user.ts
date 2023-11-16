export interface User {
  userId: number,
  firstName: string,
  middleName?: string,
  lastName: string,
  emailAddress: string,
  phone?: string,
  modifiedDate: Date,
  userName?: string,
  isAdmin: boolean,
  oldCustomerId2: number
}