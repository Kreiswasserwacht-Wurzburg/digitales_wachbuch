<script setup lang="ts">

import SentryDisplay from './SentryDisplay.vue';
import SentryStart from './SentryStart.vue';
import GuardsList from './GuardsList.vue';
import LogBookEntryList from '@/components/logBook/LogBookEntryList.vue';
import { useSentryStore } from '@/store/sentry'
import { storeToRefs } from 'pinia'

// const sentry: Ref<Sentry | null> = ref(null)

import { useI18n } from 'vue-i18n'
import { onMounted } from 'vue';

const { t } = useI18n({
    useScope: 'global'
})

const store = useSentryStore()

const { active, loading, sentry} = storeToRefs(store)

onMounted(() => {
    store.getActiveSentry()
})

</script>

<template>
    <div :class="{ 'col-md-6': active, 'col-md-9': !active }">
        <template v-if="!loading">
            <div class="card">
                <div class="card-body">
                    <SentryDisplay v-model:sentry="sentry" v-if="active" />
                    <SentryStart v-else @created="sentry = $event" />
                </div>
            </div>
            <div class="card" v-if="active">
                <div class="card-body">
                    <LogBookEntryList v-model:from="sentry.start" />
                </div>
            </div>
        </template>
    </div>
    <div class="col-md-3" v-if="active">
        <div class="card">
            <div class="card-body">
                <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-circle-fill" fill="currentColor"
                    xmlns="http://www.w3.org/2000/svg">
                    <circle cx="8" cy="8" r="8" />
                </svg>
                {{ t('sentry.activeGuards') }}

                <GuardsList v-model:guards="sentry.guards" />

            </div>
        </div>
    </div>
</template>