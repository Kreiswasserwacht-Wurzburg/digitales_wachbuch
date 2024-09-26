<script setup lang="ts">

import type { Sentry } from '@/models/sentry';
import { useSentryStore } from '@/store/sentry'
import { DateTime } from 'luxon'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faArrowsRotate, faSquarePhoneFlip } from '@fortawesome/free-solid-svg-icons'
import { storeToRefs } from 'pinia'
import ModalDialog from '@/components/dialog/ModalDialog.vue'
import { ref } from 'vue'

library.add(faArrowsRotate, faSquarePhoneFlip)

import { useI18n } from 'vue-i18n'
const regDialog = ref<InstanceType<typeof ModalDialog> | null>()
const svSwitchDialog = ref<InstanceType<typeof ModalDialog> | null>()
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


const formattedRegistration = ref<string>(<string>props.sentry.registration?.toLocaleString().slice(0,16))
const selectedRegistration = ref<string>(formattedRegistration.value)

async function submit(): Promise<void> {
    var res = await store.finishSentry({
        id: props.sentry.id,
        finish: DateTime.now()
    })

    emit("update:sentry", undefined);
}

function regOnSubmit() {
  
  const validation = () => {
    var ret = store.registerSentry({
        id: props.sentry.id,
        registration: <DateTime>DateTime.fromISO(selectedRegistration.value),
    })
    return true
  }

  if (validation()) {
    formattedRegistration.value = selectedRegistration.value
    regDialog.value?.close()
  }
}

function regDialogOpen() {
  selectedRegistration.value = formattedRegistration.value
  regDialog.value?.open()
}

function svSwitchOnSubmit() {
  
  const validation = () => {
    return true
  }

  if (validation()) {
    svSwitchDialog.value?.close()
  }
}

function svSwitchDialogOpen() {

  svSwitchDialog.value?.open()
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
                <td>{{ d(sentry.start.toLocaleString(), "shortDateTime") }}</td>
                <td><template v-if="formattedRegistration">
                    {{ d(formattedRegistration.toLocaleString(), "shortDateTime") }} 
                    </template>
                    <a class="btn btn-sm" @click="regDialogOpen"><font-awesome-icon
                                :icon="['fa', 'square-phone-flip']" /></a>
                </td>
                <td>{{ sentry.organisation?.name }}</td>
                <td>{{ `${activeSupervisor?.firstName} ${activeSupervisor?.lastName}` }} <a class="btn btn-sm" href="#" @click="svSwitchDialogOpen"><font-awesome-icon :icon="['fa', 'arrows-rotate']" /></a>
                </td>
            </tr>
        </tbody>
    </table>

    <button type="submit" class="btn btn-primary" @click.prevent="submit()">{{ t('sentry.stopAction') }}</button>

    <div class="modal" id="changeSupervisorModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title">{{ t('sentry.changeSupervisor') }}</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <label for="newSupervisor" class="form-label col-sm-4">{{ t('sentry.supervisor') }}</label>
                        <div class="col-sm-8">
                            <select id="newSupervisor" class="form-select">
                                <option disabled :value="null">{{ t('common.general.select') }}</option>
                            </select>
                        </div>
                    </div>
                    <div class="row">
                        <label for="supervisorChangeTime" class="form-label col-sm-4">{{ t('common.general.from') }}</label>
                        <div class="col-sm-8">
                            <input id="supervisorChangeTime" type="datetime-local" class="form-control" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">{{ t('common.buttons.cancel')}}</button>
                    <button type="button" class="btn btn-primary">{{ t('common.buttons.submit') }}</button>
                </div>
            </div>
        </div>
    </div>
    <ModalDialog ref="regDialog" @submit="regOnSubmit">
        <template #title>{{ t('sentry.registerControlCentre') }}</template>
        <template #body>
            {{ t('sentry.callControlCentre') }}: <a href="tel:+499311234567">0931 / 1234567</a>
            <hr />
            <input type="datetime-local" class="form-control" v-model="selectedRegistration"/>
        </template>
    </ModalDialog>

    <ModalDialog ref="svSwitchDialog" @submit="svSwitchOnSubmit">
        <template #title>{{ t('sentry.changeSupervisor') }}</template>
        <template #body>
            <div class="row">
                <label for="newSupervisor" class="form-label col-sm-4">{{ t('sentry.supervisor') }}</label>
                <div class="col-sm-8">
                    <select id="newSupervisor" class="form-select">
                        <option disabled :value="null">{{ t('common.general.select') }}</option>
                    </select>
                </div>
            </div>
            <div class="row">
                <label for="supervisorChangeTime" class="form-label col-sm-4">{{ t('common.general.from') }}</label>
                <div class="col-sm-8">
                    <input id="supervisorChangeTime" type="datetime-local" class="form-control" />
                </div>
            </div>
        </template>
    </ModalDialog>

</template>