<script setup lang="ts">

import type { Sentry } from './sentry';
import { DateTime } from 'luxon'

const props  = defineProps<{
    sentry: Sentry
}>()

</script>

<template>
    <dl>
        <dt>Start</dt>
        <dd>{{ sentry.start.toLocaleString(DateTime.DATETIME_SHORT) }}</dd>
        <dt>Registration</dt>
        <dd>{{ sentry.registration?.toLocaleString(DateTime.DATETIME_SHORT) }}</dd>
        <dt>End</dt>
        <dd>{{ sentry.end?.toLocaleString(DateTime.DATETIME_SHORT) }}</dd>
        <dt>Organisation</dt>
        <dd>{{ sentry.organisation?.name }}</dd>
    </dl>
</template>