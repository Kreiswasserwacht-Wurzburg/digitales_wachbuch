<script setup lang="ts">
import ModalDialog from '@/components/dialog/ModalDialog.vue'
import { ref } from 'vue'

const dialog = ref<InstanceType<typeof ModalDialog> | null>()

const i = ref<number>(0);

function onSubmit() {
  const validation = () => {
    console.log("Doing some fancy validation")
    return true
  }

  if (validation()) {
    dialog.value?.close()
  }
}

function dialogOpen() {
  i.value++
  dialog.value?.open()
}
</script>

<template>
  <div class="about">
    <h1>This is an about page</h1>
    <button @click="dialogOpen">Show</button>
  </div>

  <ModalDialog ref="dialog" @submit="onSubmit">
    <template #title>Lorem ipsum</template>
    <template #body>Dolor sit amet: {{ i }}</template>
  </ModalDialog>
</template>

<style></style>
