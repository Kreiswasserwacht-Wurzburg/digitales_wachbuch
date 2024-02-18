import { DateTime } from "luxon";

export interface Person {
    id: string,
    firstName: string,
    lastName: string
}

export interface Organisation {
    id: string,
    name: string,
    number: number,
    members: Person[]
}

export interface SentryStart {
    start: DateTime,
    registration?: DateTime,
    supervisor?: Person,
    organisation?: Organisation
}

export interface GuardService {
    start: DateTime,
    end: DateTime,
    guard: Person
}

export interface Sentry {
    id: string,
    start: DateTime | string,
    registration?: DateTime | string,
    end?: DateTime | string,
    supervisors: GuardService[],
    organisation: Organisation,
    guards: GuardService[]
}