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
    warnings: []
}