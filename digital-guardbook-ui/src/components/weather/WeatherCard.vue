<script setup lang="ts">
import { onMounted, ref } from 'vue'
import type { Ref } from 'vue'
import WeatherApiService from './weather.api';
import type WeatherInfo from './weatherInfo';
import { DateTime } from 'luxon';
/* import the fontawesome core */
import { library } from '@fortawesome/fontawesome-svg-core'
import { faTemperatureHigh, faTemperatureLow, faWind, faUmbrella } from '@fortawesome/free-solid-svg-icons'

import WeatherIcon from './WeatherIcon.vue';

const refreshDelayMinutes = ref(15);

const delay = (ms: number) => new Promise(res => setTimeout(res, ms));

library.add(faTemperatureHigh, faTemperatureLow, faWind, faUmbrella)

const props = defineProps({
  stationId: {
    type: Number,
    required: true
  }
})

const weatherApiService = new WeatherApiService

const weatherInfo: Ref<WeatherInfo | null> = ref(null);

async function fetchData() {
  const [error, weather] = await weatherApiService.getWeatherInfo(props.stationId);

  if (error) {
    console.error(error);
  }
  else {
    weatherInfo.value = weather;
  }

  await delay(refreshDelayMinutes.value * 60 * 1000)
  fetchData()
}

onMounted(() => {
  fetchData();
})
</script>

<template>
  <div class="card">
    <div class="card-body">
      Wetter
      <div v-if="weatherInfo">
        <div>
          <h2>
            <WeatherIcon :iconNumber="weatherInfo.icon" :isDay="weatherInfo.isDay" /> {{ weatherInfo.temperature.current
            }} °C
          </h2>
          <font-awesome-icon :icon="['fas', 'temperature-low']" /> {{ weatherInfo.temperature.from }} °C |
          <font-awesome-icon :icon="['fas', 'temperature-high']" /> {{ weatherInfo.temperature.to }} °C
        </div>
        <div>
          <p>
            <font-awesome-icon :icon="['fas', 'umbrella']" /> {{ weatherInfo.rainfall }} mm<br /> <font-awesome-icon
              :icon="['fas', 'wind']" />{{ weatherInfo.wind.speed }} km/h | {{
                weatherInfo.wind.gust }} km/h Böen
          </p>
        </div>
        <p>
          <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-circle-fill"
            fill="{{weatherInfo.warnings.length > 0 ? 'red' : 'green' }}" xmlns="http://www.w3.org/2000/svg">
            <circle cx="8" cy="8" r="8" />
          </svg>
        <div v-if="weatherInfo.warnings.length">
          <ul>
            <li v-for="warning in weatherInfo.warnings">
              <b>{{ warning.headline }}</b> <br />
              Von {{ warning.start.toLocaleString(DateTime.DATETIME_SHORT) }} bis {{
                warning.end.toLocaleString(DateTime.DATETIME_SHORT) }} <br />
              <small>{{ warning.description }}</small>
            </li>
          </ul>
        </div>
        <div v-if="!weatherInfo.warnings.length">
          keine Unwetterwarnungen
        </div>
        </p>
      </div>
    </div>
  </div>
</template>