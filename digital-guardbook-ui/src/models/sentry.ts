import { DateTime } from "luxon";
import type { Person } from `./person`;
import type { Organisation } from `./organisation`;

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

export interface SentryFinish {
    id: string,
    finish: DateTime
}