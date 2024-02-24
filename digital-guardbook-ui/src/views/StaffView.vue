<script setup lang="ts">
import { useI18n } from 'vue-i18n'

const { t } = useI18n({
    useScope: 'global'
})

import { usePersonStore } from '@/store/person'
import { storeToRefs } from 'pinia'
import { onMounted, ref, watch } from 'vue'
import PickList from '@/components/data/PickList.vue'
import type { Person } from '@/models/person'


const store = usePersonStore()

const { loading, persons } = storeToRefs(store)

const selection = ref<Person[]>([])

onMounted(() => {
    store.fetchAll()
})

</script>

<template>
    <main>
        <div class="container-fluid">
            <div class="row my-3">
                <PickList v-model:data="persons" v-model:selection="selection" v-if="!loading && persons" class="vh-100">
                    <template #sourceTitle>Source</template>
                    <template #targetTitle>Target</template>
                    <template #item="{firstName, lastName}">
                        {{ firstName }} {{ lastName }}
                    </template>
                </PickList>
            </div>
        </div>
    </main>
</template>