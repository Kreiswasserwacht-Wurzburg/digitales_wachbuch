import { DateTime } from "luxon";

export interface Person {
    id: string,
    firstName: string,
    lastName: string
}

export interface Organisation {
    id: string,
    name: string,
    number: number
}

export default interface SentryStart {
    start: DateTime,
    registration?: DateTime,
    supervisor: Person,
    organisation: Organisation
}