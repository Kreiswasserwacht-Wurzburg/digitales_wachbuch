<script setup lang="ts">
import WeatherCard from "@/components/weather/WeatherCard.vue"
import StationCard from "@/components/station/StationCard.vue"
import SentryCard from "@/components/sentry/SentryCard.vue"

import { useI18n } from 'vue-i18n'

const {t} = useI18n({
  useScope: 'global'
})
</script>

<template>
  <main>
    <div class="container-fluid">
      <div class="row my-3">

        <SentryCard />
        
        <div class="col-md-3 justify-content-around">
          <WeatherCard :station-id=10655 />
          <div class="card">
            <div class="card-body">
              <h5>{{ t('dashboard.controlCentre') }}</h5>
              <p>
                <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-circle-fill" fill="currentColor"
                  xmlns="http://www.w3.org/2000/svg">
                  <circle cx="8" cy="8" r="8" />
                </svg>
                ILS Musterstadt
              </p>
              <p>Funkname: ILS Musterstadt<br />Tel. 0931 / 1234567 <br>Funkgruppe: RD_XX 1234</p>
            </div>
          </div>
          <StationCard />
        </div>
      </div>
    </div>
  </main>
</template>
