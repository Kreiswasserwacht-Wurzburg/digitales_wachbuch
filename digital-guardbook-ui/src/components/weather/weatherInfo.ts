import { DateTime } from "luxon"

export class Warnings {
    headline: string;
    start: DateTime;
    end: DateTime;
    description: string;

    constructor(headline: string, start: number, end: number, description: string) {
        this.headline = headline;
        this.start = DateTime.fromMillis(start);
        this.end = DateTime.fromMillis(end);
        this.description = description;
    }
}

export default interface WeatherInfo {
    temperature: {
        current: number,
        from: number,
        to: number
    },
    rainfall: number,
    wind: {
        speed: number,
        gust: number
    },
    icon: number,
    isDay: boolean,
    warnings: Array<Warnings>
}