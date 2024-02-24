import { defineStore } from 'pinia'
import { ref } from 'vue'
import apolloClient from '@/plugins/apollo'
import gql from 'graphql-tag'
import { LogBookEntry } from '@/models/logBook'
import { DateTime } from 'luxon'
import { type } from 'os'
import { RefSymbol } from '@vue/reactivity'

export const useLogBookStore = defineStore('logBook', () => {
    const logBookEntries = ref<LogBookEntry[]>()
    const loading = ref<Boolean>(false)
    const socket = ref<WebSocket>(new WebSocket("ws://localhost:5282/ws"));
    const dateFrom = ref<DateTime>()
    const dateTo = ref<DateTime>()

    socket.value.onmessage = function(event){
        fetchByTime(dateFrom.value, dateTo.value);
    }

    async function fetchByTime(from: DateTime, to: DateTime | null) {
        loading.value = true
        dateFrom.value = from
        dateTo.value = to

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
            },
            fetchPolicy: 'no-cache',
        })

        logBookEntries.value = data.logBookEntries
        loading.value = false
    }

    return { logBookEntries, loading, fetchByTime }
})