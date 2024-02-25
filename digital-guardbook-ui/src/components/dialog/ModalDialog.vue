<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { Modal } from 'bootstrap/js/index.esm'

export interface Options {
    backdrop: boolean | 'static',
    focus: boolean,
    keyboard: boolean
}

const dialogElem = ref<HTMLElement| null>(null)

const modal = ref<Modal>();

const open = () => (modal.value.show())
const close = () => (modal.value.hide())

defineExpose({
    open, close
})

onMounted(() => {
    modal.value = new Modal(dialogElem.value)
})
</script>

<template>
    <div class="modal" ref="dialogElem">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">
                        <slot name="title"></slot>
                    </h5>
                </div>
                <div class="modal-body">
                    <slot name="body"></slot>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" @click="close">Close</button>
                    <button type="button" class="btn btn-primary" @click="close">Save changes</button>
                </div>
            </div>
        </div>
    </div>
</template>