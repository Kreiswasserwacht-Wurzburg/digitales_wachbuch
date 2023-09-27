<script setup lang="ts">
import { useQuery, useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { ref, computed, onMounted } from 'vue';
import type { Ref } from 'vue';
import type SentryStart from './sentry';
import { DateTime } from 'luxon';
import type { Sentry } from './sentry';

const emit = defineEmits<{
    created: [value: Sentry]
}>()


const delay = (ms: number) => new Promise(res => setTimeout(res, ms));

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

const { mutate: startSentry } = useMutation(gql`
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
`)

const sentry: Ref<SentryStart> = ref({
    start: DateTime.now(),
} as SentryStart)

const start = computed(() => { return sentry.value.start.toFormat("yyyy-MM-dd'T'T") })

let manualEdit: Boolean = false;

function allDataFilled(): boolean {
    return sentry.value.start != null && sentry.value.organisation != null && sentry.value.supervisor != null;
}

async function setTime() {
    await delay(500);
    if (!manualEdit) {
        sentry.value.start = DateTime.now();
        setTime();
    }
}

async function submitForm(): Promise<void> {
    if (allDataFilled()) {
        var formattedStart = DateTime.fromISO(start?.value);

        var res = await startSentry({
            "sentry": {
                "start": formattedStart,
                "organisation": {
                    "id": sentry.value.organisation?.id,
                },
                "supervisors": [{
                    "start": formattedStart,
                    "guard": {
                        "id": sentry.value.supervisor?.id
                    }
                }]
            }
        });

        console.log(res?.data.startSentry);
        emit('created', res?.data.startSentry);
    }
}

function onTimeFocus() {
    manualEdit = true;
}

onMounted(() => { setTime() })
</script>

<template>
    <form>
        <div class="row mb-3">
            <label for="name" class="form-label col-sm-2 ">Ortsgruppe</label>
            <div class="col-sm-10">
                <select v-model="sentry.organisation" class="form-select" required v-if="!loading">
                    <option disabled :value="null">Please select one</option>
                    <option v-for="organisation in result?.organisations" :value="organisation" :key="organisation.id">{{
                        organisation.name }}</option>
                </select>
            </div>
        </div>

        <div class="row mb-3">
            <label for="name" class="form-label col-sm-2 ">Wachleiter</label>
            <div class="col-sm-10">
                <select v-model="sentry.supervisor" class="form-select" required
                    :class="{ disabled: sentry.organisation?.members }">
                    <option disabled :value="null">Please select one</option>
                    <option v-for="member in sentry.organisation?.members" :value="member" :key="member.id">{{
                        member.firstName }} {{ member.lastName }}</option>
                </select>
            </div>
        </div>

        <div class="row mb-3">
            <label for="name" class="form-label col-sm-2 ">Datum</label>
            <div class="col-sm-10">
                <input type="datetime-local" class="form-control" required v-model="start" @focus="onTimeFocus" />
            </div>
        </div>
        <button type="submit" class="btn btn-primary" @click.prevent="submitForm()"
            :class="{ disabled: !allDataFilled() }">Start</button>
    </form>
</template>