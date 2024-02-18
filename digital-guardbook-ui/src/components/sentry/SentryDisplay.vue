<script setup lang="ts">

import type { Sentry } from '@/models/sentry';
import { useSentryStore } from '@/store/sentry'
import { DateTime } from 'luxon'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faArrowsRotate, faSquarePhoneFlip } from '@fortawesome/free-solid-svg-icons'
import { storeToRefs } from 'pinia'

library.add(faArrowsRotate, faSquarePhoneFlip)

import { useI18n } from 'vue-i18n'

const store = useSentryStore()
const { activeSupervisor } = storeToRefs(store)

const { t, n, d } = useI18n({
    useScope: 'global'
})

const props = defineProps<{
    sentry: Sentry
}>()

const emit = defineEmits<{
    "update:sentry": [sentry?: Sentry]
}>()

async function submit(): Promise<void> {
    var res = await store.finishSentry({
        id: props.sentry.id,
        finish: DateTime.now()
    })

    if (res?.errors == undefined) {
        emit("update:sentry", undefined);
    }
}

function getDateTime(dt: DateTime | string): Date {
    if (typeof (dt) == typeof (DateTime)) {
        return (dt as DateTime).toJSDate();
    }
    else {
        return DateTime.fromISO(dt as string).toJSDate();
    }
}

async function saveRegistration(): Promise<void> {

    var res = await store.registerSentry({
        id: props.sentry.id,
        registration: DateTime.fromISO(props.sentry.registration).toISO(),
    })

    if (res?.errors == undefined) {

        emit("update:sentry", props.sentry); 
    }

    document.getElementById('registrationModal-close')?.click();
}

async function cancelRegistration(): Promise<void> {

    props.sentry.registration = prevRegistration;

}

</script>

<template>
    <table class="table table-borderless">
        <thead>
            <tr>
                <th>{{ t('sentry.startTime') }}</th>
                <th>{{ t('sentry.registrationTime') }}</th>
                <th>{{ t('sentry.organisation') }}</th>
                <th>{{ t('sentry.supervisor') }}</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>{{ d(getDateTime(sentry.start), "shortDateTime") }}</td>
                <td><template v-if="sentry.registration">
                    {{ d(getDateTime(sentry.registration), "shortDateTime") }} 
                    </template>
                    <a class="btn btn-sm" data-bs-toggle="modal" data-bs-target="#registrationModal"><font-awesome-icon
                                :icon="['fa', 'square-phone-flip']" /></a>
                </td>
                <td>{{ sentry.organisation?.name }}</td>
                <td>{{ `${activeSupervisor?.firstName} ${activeSupervisor?.lastName}` }} <a class="btn btn-sm" href="#" data-bs-toggle="modal"
                        data-bs-target="#changeSupervisorModal"><font-awesome-icon :icon="['fa', 'arrows-rotate']" /></a>
                </td>
            </tr>
        </tbody>
    </table>

    <button type="submit" class="btn btn-primary" @click.prevent="submit()">{{ t('sentry.stopAction') }}</button>

    <div class="modal" id="registrationModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title">Anmeldung bei der ILS</h1>
                    <button type="button" id="registrationModal-close" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    ILS anrufen: <a href="tel:+499311234567">0931 / 1234567</a>
                    <hr />
                    <input type="datetime-local" class="form-control" v-model="sentry.registration"/>
                </div>
                <div class="modal-footer">
                    <button type="button" @click="cancelRegistration" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" @click="saveRegistration" class="btn btn-primary">Save changes</button>
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