import axios, { type AxiosInstance, type AxiosRequestConfig } from 'axios';
import type WeatherInfo from './weatherInfo';

export default class WeatherApiService {
    private axiosInstance: AxiosInstance;

    constructor() {
        this.axiosInstance = axios.create({
            baseURL: "/weather",
        })
    }

    private async axiosCall<T>(config: AxiosRequestConfig): Promise<[any,T|null]> {
        try {
            const { data } = await this.axiosInstance.request<T>(config);

            return [null, data]
        }
        catch (error) {
            return [error, null];
        }
    }

    async getWeatherInfo(station: number) : Promise<[any,WeatherInfo|null]> {
        return await this.axiosCall<WeatherInfo>({
            method: 'get',
            url: `/stationOverviewExtended?stationIds=${station}`,
            transformResponse: [(dataStr) => {
                let data = JSON.parse(dataStr);
                var weatherInfo: WeatherInfo = {
                    rainfall: data[`${station}`]["days"][0]["precipitation"] / 10,
                    temperature: {
                        from: data[`${station}`]["days"][0]["temperatureMin"] / 10,
                        to: data[`${station}`]["days"][0]["temperatureMax"] / 10,
                    },
                    wind: {
                        speed: data[`${station}`]["days"][0]["windSpeed"] / 10,
                        gust: data[`${station}`]["days"][0]["windGust"] / 10,
                    },
                    warnings: data[`${station}`]["warnings"]
                };

                return weatherInfo;
            }]
        });
    }
}