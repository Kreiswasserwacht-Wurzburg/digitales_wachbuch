import { defineStore } from 'pinia'
import { computed, ref } from 'vue'
import type { Sentry, SentryStart, SentryFinish } from '@/models/sentry'
import apolloClient from '@/plugins/apollo'
import gql from 'graphql-tag'

// GraphQL response types (match the actual fields selected in queries/mutations, not the full domain models)
interface SentryGuard {
  start: string
  end?: string
  guard: { id: string; firstName: string; lastName: string }
}

interface SentrySupervisor {
  start: string
  end?: string
  guard: { firstName: string; lastName: string }
}

interface SentryResponse {
  id: string
  start: string
  end?: string
  registration?: string
  organisation: { name: string }
  supervisors: SentrySupervisor[]
  guards: SentryGuard[]
}

interface StartSentryResult {
  startSentry: SentryResponse
}

interface GetActiveSentryResult {
  activeSentry: SentryResponse
}

export const useSentryStore = defineStore('sentry', () => {
  const sentry = ref<Sentry | null>(null)
  const loading = ref<boolean>(false)
  const error = ref<string | null>(null)
  const active = computed(() => sentry.value?.id != null)
  const activeSupervisor = computed(
    () => sentry.value?.supervisors.find((x) => x.end == undefined)?.guard
  )
  const guards = computed(() => sentry.value?.guards?.map((x) => x.guard))

  async function startSentry(_sentry: SentryStart) {
    try {
      error.value = null
      const result = await apolloClient.mutate<StartSentryResult>({
        mutation: gql`
          mutation ($sentry: SentryStartType!) {
            startSentry(sentry: $sentry) {
              id
              start
              end
              registration
              organisation {
                name
              }
              supervisors {
                start
                end
                guard {
                  firstName
                  lastName
                }
              }
              guards {
                start
                end
                guard {
                  id
                  firstName
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

      // GraphQL response under-selects some fields; cast via unknown to bypass structural check
      sentry.value = result.data!.startSentry as unknown as Sentry

      return result.data!.startSentry as unknown as Sentry
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to start sentry'
      throw err
    }
  }

  async function getActiveSentry() {
    try {
      loading.value = true
      error.value = null
      const { data } = await apolloClient.query<GetActiveSentryResult>({
        query: gql`
          query {
            activeSentry {
              id
              start
              end
              registration
              organisation {
                name
              }
              supervisors {
                start
                end
                guard {
                  firstName
                  lastName
                }
              }
              guards {
                start
                end
                guard {
                  id
                  firstName
                  lastName
                }
              }
            }
          }
        `,
        fetchPolicy: 'no-cache'
      })

      // GraphQL response under-selects some fields; cast via unknown to bypass structural check
      sentry.value = data!.activeSentry as unknown as Sentry
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to fetch active sentry'
    } finally {
      loading.value = false
    }
  }

  async function finishSentry(_sentry: SentryFinish) {
    try {
      error.value = null
      await apolloClient.mutate({
        mutation: gql`
          mutation ($sentry: SentryFinishType!) {
            finishSentry(sentry: $sentry)
          }
        `,
        variables: {
          sentry: _sentry
        }
      })
      // Clear the sentry state on successful finish so UI reverts to "start" form
      sentry.value = null
    } catch (err) {
      error.value = err instanceof Error ? err.message : 'Failed to finish sentry'
      throw err
    }
  }

  return {
    active,
    loading,
    error,
    sentry,
    getActiveSentry,
    startSentry,
    finishSentry,
    activeSupervisor,
    guards
  }
})
