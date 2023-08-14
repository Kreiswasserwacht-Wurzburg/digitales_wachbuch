import axios, { type AxiosInstance, type AxiosRequestConfig } from 'axios';
import type WeatherInfo from './weatherInfo';
import { DateTime } from 'luxon';

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
                        current: data[`${station}`]["forecast1"]["temperature"][DateTime.now().hour] / 10,
                        from: data[`${station}`]["days"][0]["temperatureMin"] / 10,
                        to: data[`${station}`]["days"][0]["temperatureMax"] / 10,
                    },
                    icon:  data[`${station}`]["forecast1"]["icon"][DateTime.now().hour],
                    isDay:  data[`${station}`]["forecast1"]["isDay"][DateTime.now().hour],
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