<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import type { Ref } from 'vue';
import { DateTime } from 'luxon';
import { useSentryStore } from '@/store/sentry'
import { useOrganisationStore } from '@/store/organisation'
import type { Sentry, SentryStart } from '@/models/sentry';
import { storeToRefs } from 'pinia'

import { useI18n } from 'vue-i18n'

const { t } = useI18n({
    useScope: 'global'
})

const store = useSentryStore()
const orgStore = useOrganisationStore()

const emit = defineEmits<{
    created: [value: Sentry]
}>()

const delay = (ms: number) => new Promise(res => setTimeout(res, ms));

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

const { loading, organisations} = storeToRefs(orgStore)

async function submitForm(): Promise<void> {
    if (allDataFilled()) {
        var formattedStart = DateTime.fromISO(start?.value);

        var res = await store.startSentry({
            start: formattedStart,
            organisation: {
                id: sentry.value.organisation?.id,
            },
            supervisors: [{
                start: formattedStart,
                guard: {
                    id: sentry.value.supervisor?.id
                }
            }]
        });

        emit('created', res?.data.startSentry);
    }
}

function onTimeFocus() {
    manualEdit = true;
}

onMounted(() => { 
    setTime()
    orgStore.fetchAll()
})
</script>

<template>
    <form>
        <div class="row mb-3">
            <label for="name" class="form-label col-sm-2 ">{{ t('sentry.organisation') }}</label>
            <div class="col-sm-10">
                <select v-model="sentry.organisation" class="form-select" required v-if="!loading">
                    <option disabled :value="null">Please select one</option>
                    <option v-for="organisation in organisations" :value="organisation" :key="organisation.id">{{
                        organisation.name }}</option>
                </select>
            </div>
        </div>

        <div class="row mb-3">
            <label for="name" class="form-label col-sm-2 ">{{ t('sentry.supervisor') }}</label>
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
            <label for="name" class="form-label col-sm-2 ">{{ t('sentry.startTime') }}</label>
            <div class="col-sm-10">
                <input type="datetime-local" class="form-control" required v-model="start" @focus="onTimeFocus" />
            </div>
        </div>
        <button type="submit" class="btn btn-primary" @click.prevent="submitForm()"
            :class="{ disabled: !allDataFilled() }">{{ t('sentry.startAction') }}</button>
    </form>
</template>