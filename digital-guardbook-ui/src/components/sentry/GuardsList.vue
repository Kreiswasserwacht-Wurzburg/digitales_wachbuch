<script setup lang="ts">

import type { GuardService } from './sentry'
import { DateTime } from 'luxon'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faRightFromBracket } from '@fortawesome/free-solid-svg-icons'

library.add(faRightFromBracket)

const props = defineProps<{
    guards: GuardService[]
}>()

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
    <div class="card" v-for="guardService in props.guards">
        <div class="card-body">
            <h5 class="card-title">{{ guardService.guard.firstName }} {{ guardService.guard.lastName }}</h5>
            <h6 class="card-subtitle text-muted">{{ convertDateTimeToString(guardService.start) }}</h6>
        </div>
        <div class="card-footer text-end">
            <a class="btn btn-sm" href="#" @click.prevent><font-awesome-icon
                    :icon="['fa-solid', 'right-from-bracket']" /></a>
        </div>
    </div>
</template>