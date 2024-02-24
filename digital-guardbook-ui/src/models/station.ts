export interface Address {
    street: string,
    zipCode: string,
    city: string
}

export interface Station {
    name: string,
    address: Address
}