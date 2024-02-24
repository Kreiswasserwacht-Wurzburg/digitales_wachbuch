<script setup lang="ts">
import { useI18n } from 'vue-i18n'

const { t } = useI18n({
    useScope: 'global'
})

import { usePersonStore } from '@/store/person'
import { storeToRefs } from 'pinia'
import { onMounted } from 'vue'
import PickList from '@/components/data/PickList.vue'
import { useSentryStore } from '../store/sentry';

import { library } from '@fortawesome/fontawesome-svg-core'
import { faFloppyDisk } from '@fortawesome/free-solid-svg-icons'
import type { Person } from '@/models/person';
import { watch, computed } from 'vue';
import { onBeforeRouteLeave } from 'vue-router';

library.add(faFloppyDisk)

const store = usePersonStore()
const sentryStore = useSentryStore()
const { loading, persons } = storeToRefs(store)
const { guards } = storeToRefs(sentryStore)

let _pristine: Person[] = []
let _isPristineSet = false

onMounted(() => {
    store.fetchAll()
    sentryStore.getActiveSentry()
})

watch(guards, (newValue, oldValue) => {
    if (!_isPristineSet) {
        newValue.forEach(val => _pristine.push(Object.assign({}, val)));
        _isPristineSet = true
    }
})

var isModified = computed(() => {
    if (_pristine === guards.value) { return false }
    if (_pristine == null || guards.value == null) { return true }
    if (_pristine.length !== guards.value.length) { return true }

    for (const guard of guards.value) {
        var item = _pristine.find((x) => x.id == guard.id)

        if (!item) {
            return true
        }
    }

    return false
})

onBeforeRouteLeave((to, from)=> {
    return confirm('Do you really want to leave? you have unsaved changes!')
})

function onBeforeUnload(ev: BeforeUnloadEvent) {
    if (isModified.value)
    {
        ev.preventDefault()
        ev.returnValue = "Unsaved changes"
    }
}

window.onbeforeunload = onBeforeUnload

</script>

<template>
    <main>
        <div class="container-fluid">
            <div class="row my-3">
                <PickList v-model:data="persons" v-model:selection="guards" v-if="!loading && persons">
                    <template #targetTitle>
                        <button class="float-end btn btn-outline-secondary" type="button"><font-awesome-icon
                                :icon="['fa', 'floppy-disk']" /> </button>
                        <h2>{{ t('sentry.activeGuards') }}</h2>
                        <span class="float-none"></span>
                    </template>
                    <template #item="{ firstName, lastName }">
                        {{ firstName }} {{ lastName }}
                    </template>
                </PickList>
            </div>
        </div>
    </main>
</template>