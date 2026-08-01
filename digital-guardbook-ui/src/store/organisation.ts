import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Organisation } from '@/models/organisation'
import apolloClient from '@/plugins/apollo'
import gql from 'graphql-tag'

interface FetchAllResult {
  organisations: Organisation[]
}

export const useOrganisationStore = defineStore('organisation', () => {
  const organisations = ref<Organisation[]>()
  const loading = ref<boolean>(false)

  async function fetchAll() {
    loading.value = true
    const { data } = await apolloClient.query<FetchAllResult>({
      query: gql`
        query {
          organisations {
            id
            name
            number
            members {
              id
              firstName
              lastName
            }
          }
        }
      `
    })

    organisations.value = data!.organisations

    loading.value = false
  }

  return { organisations, loading, fetchAll }
})
