<script setup lang="ts">

import { useMutation } from '@vue/apollo-composable';
import type { Sentry } from './sentry';
import gql from 'graphql-tag'
import { DateTime } from 'luxon'
import SentryCard from './SentryCard.vue';

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
</script>

<template>
    <dl>
        <dt>Id</dt>
        <dd>{{ sentry.id }}</dd>
        <dt>Start</dt>
        <dd>{{ sentry.start.toLocaleString(DateTime.DATETIME_SHORT) }}</dd>
        <dt>Registration</dt>
        <dd>{{ sentry.registration?.toLocaleString(DateTime.DATETIME_SHORT) }}</dd>
        <dt>End</dt>
        <dd>{{ sentry.end?.toLocaleString(DateTime.DATETIME_SHORT) }}</dd>
        <dt>Organisation</dt>
        <dd>{{ sentry.organisation?.name }}</dd>
    </dl>


    <button type="submit" class="btn btn-primary" @click.prevent="submit()">Wachdienst beenden</button>
</template>