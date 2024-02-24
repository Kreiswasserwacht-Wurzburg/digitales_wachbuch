import { defineStore } from 'pinia'
import { ref } from 'vue'
import { Organisation } from '@/models/organisation'
import apolloClient from '@/plugins/apollo'
import gql from 'graphql-tag'

export const useOrganisationStore = defineStore('organisation', () => {
    const organisations = ref<Organisation[]>()
    const loading = ref<Boolean>(false)

    async function fetchAll() {
        loading.value = true
        const { data } = await apolloClient.query({
            query: gql`
                query {
                    organisations {
                        id
                        name
                        members {
                            id
                            firstName
                            lastName
                        }
                    }
                }
            `
        })

        organisations.value = data.organisations

        loading.value = false
    }

    return { organisations, loading, fetchAll }
})