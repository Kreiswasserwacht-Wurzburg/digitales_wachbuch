<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { usePersonStore } from '@/store/person'
import { storeToRefs } from 'pinia'
import PickList from '@/components/data/PickList.vue'
import { useSentryStore } from '../store/sentry';
import { library } from '@fortawesome/fontawesome-svg-core'
import { faFloppyDisk } from '@fortawesome/free-solid-svg-icons'
import type { Person } from '@/models/person';
import { ref, watch, computed, onMounted } from 'vue'
import { onBeforeRouteLeave } from 'vue-router'
import { watch, computed } from 'vue';
import { onBeforeRouteLeave } from 'vue-router';

library.add(faFloppyDisk)

const { t } = useI18n({
    useScope: 'global'
})

const store = usePersonStore()
const sentryStore = useSentryStore()
const { loading, persons } = storeToRefs(store)
const dialog = ref<InstanceType<typeof ModalDialog> | null>()

const { guards } = storeToRefs(sentryStore)

const localGuardList = ref<Array<Person>>([])

let _pristine: Person[] = []
let _isPristineSet = false

onMounted(() => {
    sentryStore.getActiveSentry()
    store.fetchAll()
})

watch(guards, (newValue, oldValue) => {
    if (!_isPristineSet) {
        newValue.forEach(val => _pristine.push(Object.assign({}, val)));
        _isPristineSet = true
    }

    localGuardList.value = newValue
})

var isModified = computed(() => {
    if (_pristine === localGuardList.value) { return false }
    if (_pristine == null || localGuardList.value == null) { return true }
    if (_pristine.length !== localGuardList.value?.length) { return true }

    for (const guard of localGuardList.value) {
        var item = _pristine.find((x) => x.id == guard.id)

        if (!item) {
            return true
        }
    }

    return false
})

onBeforeRouteLeave((to, from) => {
    return confirm('Do you really want to leave? you have unsaved changes!')
})

function onBeforeUnload(ev: BeforeUnloadEvent) {
    if (isModified.value) {
        ev.preventDefault()
        ev.returnValue = "Unsaved changes"
    }
}

window.onbeforeunload = onBeforeUnload

var addedGuards = computed(() => {
    var result = Array<Person>()

    if (localGuardList.value) {
        for (const guard of localGuardList.value) {
            var item = _pristine.find((x) => x.id == guard.id)
            if (!item) {
                result.push(guard)
            }
        }

    }
    return result
})

var removedGuards = computed(() => {
    var result = Array<Person>()

    if (localGuardList.value) {
        for (const guard of _pristine) {
            var item = localGuardList.value.find((x) => x.id == guard.id)
            if (!item) {
                result.push(guard)
            }
        }
    }

    return result
})

function save() {

}
</script>

<template>
    <main>
        <div class="container-fluid">
            <div class="row my-3">
                <PickList :dataSource="persons" v-model:selection="localGuardList" v-if="!loading && persons && guards">
                    <template #targetTitle>
                        <button class="float-end btn btn-outline-secondary" type="button" @click="save"><font-awesome-icon
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