<script lang="ts">
import { defineComponent } from 'vue'
import type { PropType } from 'vue'
import WeatherApiService from './weather.api.ts';
import type WeatherInfo from './weatherInfo.ts';

const weatherApiService = new WeatherApiService

export default defineComponent({
  props: {
    stationId: {
      type: Number,
      required: true
    }
  },
  data() {
    return {
      weatherInfo: null
    }
  },
  created() {
    this.fetchData();
  },
  methods: {
    async fetchData() {
      const [error, weather] = await weatherApiService.getWeatherInfo(this.stationId);

      if (error) {
        console.error(error);
      }
      else {
        this.weatherInfo = weather;
      }
    }
  }
})
</script>

<template>
  <div class="card">
    <div class="card-body">
      Wetter
      <div v-if="weatherInfo">
        <div>
          <h2>{{ this.weatherInfo.temperature.from }} °C | {{ this.weatherInfo.temperature.to }} °C </h2>
        </div>
        <div>
          <p>{{ this.weatherInfo.rainfall }} mm Niederschlag<br /> {{ this.weatherInfo.wind.speed }} km/h Wind | {{
            this.weatherInfo.wind.gust }} km/h Böen</p>
        </div>
        <p>
          <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-circle-fill" fill="currentColor"
            xmlns="http://www.w3.org/2000/svg">
            <circle cx="8" cy="8" r="8" />
          </svg>
        <div v-if="this.weatherInfo.warnings.length">
          Warnungen!!!
        </div>
        <div v-if="!this.weatherInfo.warnings.length">
          keine Unwetterwarnungen
        </div>
        </p>
      </div>
    </div>
  </div>
</template>