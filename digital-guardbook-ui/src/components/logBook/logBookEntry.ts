import { DateTime } from "luxon";

export interface LogBookEntry {
    time: DateTime,
    author: string,
    message: string
}