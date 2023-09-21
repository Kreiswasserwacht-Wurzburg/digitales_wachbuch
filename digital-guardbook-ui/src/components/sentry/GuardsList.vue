<script setup lang="ts">

import type { GuardService } from './sentry'
import { DateTime } from 'luxon'

const props = defineProps<{
    guards: GuardService[]
}>()

</script>

<template>
    <div class="card" v-for="guardService in props.guards">
        <div class="card-body">
            <h5 class="card-title">{{ guardService.guard.firstName }} {{ guardService.guard.lastName }}</h5>
            <h6 class="card-subtitle text-muted">{{ guardService.start.toLocaleString(DateTime.TIME_24_SIMPLE) }}</h6>
        </div>
    </div>
</template>