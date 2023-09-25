<script setup lang="ts">

import { useMutation } from '@vue/apollo-composable';
import type { Sentry } from './sentry';
import gql from 'graphql-tag'
import { DateTime } from 'luxon'
import { computed } from 'vue';

const props = defineProps<{
    sentry: Sentry
}>()

const emit = defineEmits<{
    "update:sentry": [sentry?: Sentry]
}>()

const { mutate: finishSentry } = useMutation(gql`
    mutation ($sentry: SentryFinishType!) {
        finishSentry(sentry: $sentry)
    }
`)

const supervisor = computed(() => {
    let activeSupervisor = props.sentry.supervisors.find(x => x.end == undefined)?.guard;
    return `${activeSupervisor?.firstName} ${activeSupervisor?.lastName}`
})

async function submit(): Promise<void> {
    var res = await finishSentry({
        "sentry": {
            "id": props.sentry.id,
            "finish": DateTime.now().toString()
        }
    })

    if (res?.errors == undefined) {
        emit("update:sentry", undefined);
    }
}


function convertDateTimeToString(dt: DateTime | string | undefined): string {
    if (dt == undefined) {
        return "";
    }
    else if (typeof (dt) == typeof (DateTime)) {
        return dt.toLocaleString(DateTime.DATETIME_SHORT);
    }
    else {
        return DateTime.fromISO(dt as string).toLocaleString(DateTime.DATETIME_SHORT)
    }
}
</script>

<template>
    <table class="table table-borderless">
        <thead>
            <tr>
                <th>Start</th>
                <th>Registration</th>
                <th>Organisation</th>
                <th>Wachleiter</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>{{ convertDateTimeToString(sentry.start) }}</td>
                <td>{{ convertDateTimeToString(sentry.registration) }}</td>
                <td>{{ sentry.organisation?.name }}</td>
                <td>{{ supervisor }}</td>
            </tr>
        </tbody>
    </table>

    <button type="submit" class="btn btn-primary" @click.prevent="submit()">Wachdienst beenden</button>
</template>