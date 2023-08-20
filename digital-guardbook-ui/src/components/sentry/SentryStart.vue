<script setup lang="ts">
import { useQuery, useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { Ref, ref } from 'vue';
import type SentryStart from './sentry';
import { DateTime } from 'luxon';


const { result, loading } = useQuery(gql`
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
`)

const {mutate: startSentry} = useMutation(gql`
    mutation ($sentry: SentryStartType!) {
        startSentry(sentry: $sentry) {
            start
            organisation {
                id
            }
            supervisors {
                guard {
                    id
                }
            }
        }
    }
`)

const sentry: Ref<SentryStart> = ref({
    start: DateTime.now(),
    organisation: null,
    supervisor: null,
} as SentryStart)
</script>

<template>
    <form>
        <div class="row mb-3">
            <label for="name" class="form-label col-sm-2 ">Ortsgruppe</label>
            <div class="col-sm-10">
                <select v-model="sentry.organisation" class="form-select" required v-if="!loading">
                    <option disabled :value="null">Please select one</option>
                    <option v-for="organisation in result?.organisations" :value="organisation" :key="organisation.id">{{ organisation.name }}</option>
                </select>
            </div>
        </div>

        <div class="row mb-3">
            <label for="name" class="form-label col-sm-2 ">Wachleiter</label>
            <div class="col-sm-10">
                <select v-model="sentry.supervisor" class="form-select" required v-if="sentry.organisation?.members">
                    <option disabled :value="null">Please select one</option>
                    <option v-for="member in sentry.organisation?.members" :value="member" :key="member.id">{{ member.firstName }} {{ member.lastName }}</option>
                </select>
            </div>
        </div>

        <div class="row mb-3">
            <label for="name" class="form-label col-sm-2 ">Datum</label>
            <div class="col-sm-10">
                <input type="datetime-local" class="form-control" required v-model="sentry.start"/>
            </div>
        </div>
        <button type="submit" class="btn btn-primary" @click.prevent>Start</button>
    </form>
</template>