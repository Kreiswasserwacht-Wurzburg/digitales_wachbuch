import type {Person} from './person'

export interface Organisation {
    id: string,
    name: string,
    number: number,
    members: Person[]
}
