import { defineStore } from 'pinia'
import { ref } from 'vue'
import apolloClient from '@/plugins/apollo'
import gql from 'graphql-tag'
import type { LogBookEntry } from '@/models/logBook'
import { DateTime } from 'luxon'

export const useLogBookStore = defineStore('logBook', () => {
    const logBookEntries = ref<LogBookEntry[]>()
    const loading = ref<Boolean>(false)

    async function fetchByTime(from: DateTime, to: DateTime | undefined) {
        loading.value = true
        const { data } = await apolloClient.query({
            query: gql`query ($from: String!, $to: String) {
                logBookEntries(from: $from, to: $to) {
                    time
                    author
                    message
                }
            }`,
            variables: {
                from,
                to
            }
        })
        logBookEntries.value = data.logBookEntries
        loading.value = false
    }

    return { logBookEntries, loading, fetchByTime }
})