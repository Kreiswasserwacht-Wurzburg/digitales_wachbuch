import { defineStore } from 'pinia'
import { computed, ref } from 'vue'
import type { Sentry, SentryStart } from '@/models/sentry'
import apolloClient from '@/plugins/apollo'
import gql from 'graphql-tag'

export const useSentryStore = defineStore('sentry', () => {
    const sentry = ref<Sentry | null>(null)
    const loading = ref<Boolean>(false)
    const active = computed(() => sentry.value?.id != null)

    async function startSentry(_sentry: SentryStart) {
        const result = await apolloClient.mutate({
            mutation: gql`
                mutation ($sentry: SentryStartType!) {
                    startSentry(sentry: $sentry) {
                        id,
                        start,
                        end,
                        registration,
                        organisation {
                            name
                        },
                        supervisors {
                            start,
                            end,
                            guard {
                                firstName
                                lastName
                            }
                        },
                        guards {
                            start,
                            end,
                            guard {
                                firstName,
                                lastName
                            }
                        }
                    }
                }
            `,
            variables: {
                sentry: _sentry
            }
        })

        sentry.value = result.data.startSentry
    }

    async function getActiveSentry() {
        loading.value = true
        const { data } = await apolloClient.query({
            query: gql`query {
                activeSentry {
                    id,
                    start,
                    end,
                    registration,
                    organisation {
                        name
                    },
                    supervisors {
                        start,
                        end,
                        guard {
                            firstName
                            lastName
                        }
                    },
                    guards {
                        start,
                        end,
                        guard {
                            firstName,
                            lastName
                        }
                    }
                }
            }`,
            fetchPolicy: 'no-cache'
        })
    
        sentry.value = data.activeSentry
        loading.value = false
    }

    return { active, loading, sentry, getActiveSentry, startSentry }
})