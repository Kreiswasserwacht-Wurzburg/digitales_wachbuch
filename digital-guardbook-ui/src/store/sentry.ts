import { defineStore } from 'pinia'
import { computed, ref } from 'vue'
<<<<<<< HEAD
import type { Sentry, SentryStart, SentryFinish } from '@/models/sentry'
=======
import { DateTime } from 'luxon'
import type { Sentry, SentryStart, SentryFinish, SentryRegister } from '@/models/sentry'
>>>>>>> ba4e716 (Squashed commit of the following:)
import apolloClient from '@/plugins/apollo'
import gql from 'graphql-tag'

export const useSentryStore = defineStore('sentry', () => {
    const sentry = ref<Sentry | null>(null)
    const loading = ref<Boolean>(false)
    const active = computed(() => sentry.value?.id != null)
    const activeSupervisor = computed(() => sentry.value?.supervisors.find(x => x.end == undefined)?.guard)

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
<<<<<<< HEAD

        sentry.value = data.activeSentry
=======
    
        sentry.value = data.activeSentry;
        // Date conversion for input fields if registration is set
        if (sentry._value != null && sentry.value.registration != null) {
            sentry.value.registration = DateTime.fromISO(sentry.value.registration).toISO().toString().slice(0,16)
        }
>>>>>>> ba4e716 (Squashed commit of the following:)
        loading.value = false
    }

    async function finishSentry(_sentry: SentryFinish) {
        const result = apolloClient.mutate({
            mutation: gql`
                mutation ($sentry: SentryFinishType!) {
                    finishSentry(sentry: $sentry)
                }
            `,
            variables: {
                sentry: _sentry
            }
        })
    }

<<<<<<< HEAD
    return { active, loading, sentry, getActiveSentry, startSentry, finishSentry, activeSupervisor }
=======
    async function registerSentry(_sentry: SentryRegister) {
        const result = apolloClient.mutate({
            mutation: gql`
                mutation ($sentry: SentryRegisterType!) {
                    registerSentry(sentry: $sentry)
                }
            `,
            variables: {
                sentry: _sentry,
            }
        })
    }

    return { active, loading, sentry, getActiveSentry, startSentry, finishSentry, registerSentry, activeSupervisor }
>>>>>>> ba4e716 (Squashed commit of the following:)
})