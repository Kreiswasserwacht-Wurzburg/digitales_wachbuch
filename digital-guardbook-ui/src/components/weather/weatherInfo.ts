export default interface WeatherInfo {
    temperature: {
        from: number,
        to: number
    },
    rainfall: number,
    wind: {
        speed: number,
        gust: number
    }
    warnings: []
}