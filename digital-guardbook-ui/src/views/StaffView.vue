<script setup lang="ts">
import { useI18n } from 'vue-i18n'

const { t } = useI18n({
    useScope: 'global'
})

import { usePersonStore } from '@/store/person'
import { storeToRefs } from 'pinia'
import { onMounted, ref } from 'vue'
import PickList from '@/components/data/PickList.vue'
import { useSentryStore } from '../store/sentry';


const store = usePersonStore()
const sentryStore = useSentryStore()
const { loading, persons } = storeToRefs(store)
const { guards } = storeToRefs(sentryStore)

onMounted(() => {
    store.fetchAll()
    sentryStore.getActiveSentry()
})

</script>

<template>
    <main>
        <div class="container-fluid">
            <div class="row my-3">
                <PickList v-model:data="persons" v-model:selection="guards" v-if="!loading && persons" class="vh-100">
                    <template #sourceTitle>Source</template>
                    <template #targetTitle>Target</template>
                    <template #item="{ firstName, lastName }">
                        {{ firstName }} {{ lastName }}
                    </template>
                </PickList>
            </div>
        </div>
    </main>
</template>