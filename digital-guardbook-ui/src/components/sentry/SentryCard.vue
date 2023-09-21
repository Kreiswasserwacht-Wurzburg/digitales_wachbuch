<script setup lang="ts">
import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { ref, watch } from 'vue';
import SentryDisplay from './SentryDisplay.vue';
import SentryStart from './SentryStart.vue';
import GuardsList from './GuardsList.vue';
import type { Sentry } from './sentry';

// const sentry: Ref<Sentry | null> = ref(null)


const { result: query, loading } = useQuery(gql`
    query{
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
    }
`)

const sentry = ref<Sentry>();

watch(query, async (newValue) => {
    sentry.value = newValue.activeSentry
})

</script>

<template>
    <div :class="{'col-md-6': sentry, 'col-md-9': !sentry}">
        <div class="card">
            <div class="card-body">
                <div class="card-body">
                    <template v-if="!loading">
                        <SentryDisplay v-model:sentry="sentry" v-if="sentry" />
                        <SentryStart v-else @created="sentry = $event" />
                    </template>
                </div>
            </div>

        </div>

    </div>
    <div class="col-md-3" v-if="sentry">
        <div class="card">
            <div class="card-body">
                <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-circle-fill" fill="currentColor"
                    xmlns="http://www.w3.org/2000/svg">
                    <circle cx="8" cy="8" r="8" />
                </svg>
                Aktive Wachmannschaft

                <GuardsList v-model:guards="sentry.guards"/>

            </div>
        </div>
    </div>
</template>