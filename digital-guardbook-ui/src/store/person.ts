import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Person } from '@/models/person'
import apolloClient from '@/plugins/apollo'
import gql from 'graphql-tag'

export const usePersonStore = defineStore('person', () => {
  const persons = ref<Person[]>()
  const loading = ref<boolean>(false)

  async function fetchAll() {
    loading.value = true
    const { data } = await apolloClient.query({
      query: gql`
        query {
          persons {
            id
            firstName
            lastName
          }
        }
      `
    })

    persons.value = (data as any).persons

    loading.value = false
  }

  return { persons, loading, fetchAll }
})
