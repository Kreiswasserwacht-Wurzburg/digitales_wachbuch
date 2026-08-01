import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { Station } from '@/models/station'
import apolloClient from '@/plugins/apollo'
import gql from 'graphql-tag'

export const useStationStore = defineStore('station', () => {
  const station = ref<Station>()
  const loading = ref<boolean>(false)

  const logo = computed<URL>(
    () =>
      new URL(`https://meine.wasserwacht.bayern/logogenerator/H.php?kv=${station.value?.name}&ov=`)
  )

  async function fetch() {
    loading.value = true

    const { data } = await apolloClient.query({
      query: gql`
        query {
          station {
            name
            address {
              street
              city
              zipCode
            }
          }
        }
      `
    })

    station.value = (data as any).station

    loading.value = false
  }

  return { station, loading, logo, fetch }
})
