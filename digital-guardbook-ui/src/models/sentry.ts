import { DateTime } from "luxon";
import type { Person } from './person';
import type { Organisation } from './organisation';

export interface SentryStart {
    start: DateTime,
    registration?: DateTime,
    supervisors?: [{
        start: DateTime,
        guard: {
            id: string
        }
    }],
    organisation?: {
        id: string
    }
}

export interface GuardService {
    start: DateTime,
    end: DateTime,
    guard: Person
}

export interface Sentry {
    id: string,
    start: DateTime,
    registration?: DateTime,
    end?: DateTime,
    supervisors: GuardService[],
    organisation: Organisation,
    guards: GuardService[]
}

export interface SentryFinish {
    id: string,
    finish: DateTime
}

export interface SentryRegister {
    id: string,
    registration: DateTime
}