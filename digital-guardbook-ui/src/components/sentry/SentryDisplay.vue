<script setup lang="ts">

import { useMutation } from '@vue/apollo-composable';
import type { Sentry } from './sentry';
import gql from 'graphql-tag'
import { DateTime } from 'luxon'
import { computed } from 'vue';
import { library } from '@fortawesome/fontawesome-svg-core'
import { faArrowsRotate, faSquarePhoneFlip } from '@fortawesome/free-solid-svg-icons'

library.add(faArrowsRotate, faSquarePhoneFlip)

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
                <td>{{ convertDateTimeToString(sentry.registration) }} <a class="btn btn-sm" data-bs-toggle="modal"
                        data-bs-target="#registrationModal"><font-awesome-icon :icon="['fa', 'square-phone-flip']" /></a>
                </td>
                <td>{{ sentry.organisation?.name }}</td>
                <td>{{ supervisor }} <a class="btn btn-sm" href="#" data-bs-toggle="modal"
                        data-bs-target="#changeSupervisorModal"><font-awesome-icon :icon="['fa', 'arrows-rotate']" /></a>
                </td>
            </tr>
        </tbody>
    </table>

    <button type="submit" class="btn btn-primary" @click.prevent="submit()">Wachdienst beenden</button>

    <div class="modal" id="registrationModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title">Anmeldung bei der ILS</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    ILS anrufen: <a href="tel:+499311234567">0931 / 1234567</a>
                    <hr />
                    <input type="datetime-local" class="form-control" v-model="sentry.registration" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Save changes</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal" id="changeSupervisorModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title">Wachleiter wechsel</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <label for="newSupervisor" class="form-label col-sm-4">Wachleiter</label>
                        <div class="col-sm-8">
                            <select id="newSupervisor" class="form-select">
                                <option disabled :value="null">Please select one</option>
                            </select>
                        </div>
                    </div>
                    <div class="row">
                        <label for="supervisorChangeTime" class="form-label col-sm-4">Ab</label>
                        <div class="col-sm-8">
                            <input id="supervisorChangeTime" type="datetime-local" class="form-control" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Save changes</button>
                </div>
            </div>
        </div>
    </div>
</template>