<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { usePersonStore } from '@/store/person'
import { storeToRefs } from 'pinia'
import PickList from '@/components/data/PickList.vue'
import { useSentryStore } from '../store/sentry'
import { library } from '@fortawesome/fontawesome-svg-core'
import { faFloppyDisk } from '@fortawesome/free-solid-svg-icons'
import type { Person } from '@/models/person'
import { ref, watch, computed, onMounted } from 'vue'
import { onBeforeRouteLeave } from 'vue-router'
import ModalDialog from '@/components/dialog/ModalDialog.vue'

library.add(faFloppyDisk)

const { t } = useI18n({
  useScope: 'global'
})

const store = usePersonStore()
const sentryStore = useSentryStore()
const { loading, persons } = storeToRefs(store)
const dialog = ref<InstanceType<typeof ModalDialog> | null>()

const { guards, activeSupervisor } = storeToRefs(sentryStore)

const localGuardList = ref<Array<Person>>([])

const _pristine: Person[] = []
let _isPristineSet = false

onMounted(() => {
  sentryStore.getActiveSentry()
  store.fetchAll()
})

watch(guards, (newValue, oldValue) => {
  if (!_isPristineSet && newValue) {
    newValue.forEach((val) => _pristine.push(Object.assign({}, val)))
    _isPristineSet = true
  }

  if (newValue != undefined) {
    localGuardList.value = newValue
  }
})

const isModified = computed(() => {
  if (_pristine === localGuardList.value) {
    return false
  }
  if (_pristine == null || localGuardList.value == null) {
    return true
  }
  if (_pristine.length !== localGuardList.value?.length) {
    return true
  }

  for (const guard of localGuardList.value) {
    const item = _pristine.find((x) => x.id == guard.id)

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
    ev.returnValue = 'Unsaved changes'
  }
}

window.onbeforeunload = onBeforeUnload

const addedGuards = computed(() => {
  const result = Array<Person>()

  if (localGuardList.value) {
    for (const guard of localGuardList.value) {
      const item = _pristine.find((x) => x.id == guard.id)
      if (!item) {
        result.push(guard)
      }
    }
  }
  return result
})

const removedGuards = computed(() => {
  const result = Array<Person>()

  if (localGuardList.value) {
    for (const guard of _pristine) {
      const item = localGuardList.value.find((x) => x.id == guard.id)
      if (!item) {
        result.push(guard)
      }
    }
  }

  return result
})

const disabledItems = computed(() => {
  const result = Array<Person>()

  if (activeSupervisor.value != undefined) {
    result.push(activeSupervisor.value)
  }

  return result
})

function save() {
  dialog.value?.open()
}

function onSubmit() {
  // Validate that changes can be persisted
  // TODO: Add API call to persist added/removed guards to the sentry
  dialog.value?.close()
}
</script>

<template>
  <main>
    <div class="container-fluid">
      <div class="row my-3">
        <PickList
          :dataSource="persons"
          v-model:selection="localGuardList"
          v-model:disabledItems="disabledItems"
          v-if="!loading && persons && guards"
        >
          <template #targetTitle>
            <button class="float-end btn btn-outline-secondary" type="button" @click="save">
              <font-awesome-icon :icon="['fa', 'floppy-disk']" />
            </button>
            <h2>{{ t('sentry.activeGuards') }}</h2>
            <span class="float-none"></span>
          </template>
          <template #item="{ firstName, lastName }"> {{ firstName }} {{ lastName }} </template>
        </PickList>
      </div>
    </div>
  </main>

  <ModalDialog ref="dialog" @submit="onSubmit">
    <template #title></template>
    <template #body>
      Please select a sentry start time for the following added guards:

      <table v-if="addedGuards" class="table">
        <thead>
          <tr>
            <th scope="col">{{ t('guardService.guard') }}</th>
            <th scope="col">{{ t('guardService.start') }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="guard in addedGuards">
            <th scope="row">{{ guard.firstName }} {{ guard.lastName }}</th>
            <td><input type="time" /></td>
          </tr>
        </tbody>
      </table>

      <hr />
      Please select a sentry end time for the following removed guards:

      <table v-if="removedGuards" class="table">
        <thead>
          <tr>
            <th scope="col">{{ t('guardService.guard') }}</th>
            <th scope="col">{{ t('guardService.end') }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="guard in removedGuards">
            <th scope="row">{{ guard.firstName }} {{ guard.lastName }}</th>
            <td><input type="time" /></td>
          </tr>
        </tbody>
      </table>
    </template>
  </ModalDialog>
</template>
