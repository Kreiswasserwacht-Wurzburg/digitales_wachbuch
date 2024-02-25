<script setup lang="ts">
/// <reference types="bootstrap" />
import { ref, onMounted } from 'vue'
import { Modal } from 'bootstrap'
import { useI18n } from 'vue-i18n'

const { t } = useI18n({
    useScope: 'global'
})

export interface DialogConfig {
    backdrop: boolean | 'static',
    focus: boolean,
    keyboard: boolean
}


const props = defineProps({
    withHeader: { type: Boolean, default: true },
    submitCaption: { type: String, default: 'common.buttons.submit' },
    cancelCaption: { type: String, default: 'common.buttons.cancel' },
    autoCloseOnAbort: { type: Boolean, default: true }
})

const dialogElem = ref<HTMLElement | null>(null)

const open = () => {
    if (dialogElem.value != null) {
        Modal.getInstance(dialogElem.value)?.show()
    }
}
const close = () => {
    if (dialogElem.value != null) {
        Modal.getInstance(dialogElem.value)?.hide()
            }
}

defineExpose({
    open, close
})

onMounted(() => {
const config: DialogConfig = {
        backdrop: 'static',
        focus: true,
        keyboard: true,
    }

    if (dialogElem.value != null) {
        Modal.getOrCreateInstance(dialogElem.value, config)
    }
})

const emit = defineEmits<{
    (e: 'submit'): void
    (e: 'abort'): void
}>()

function onAbort() {
    emit('abort')

    if (props.autoCloseOnAbort) {
        close()
    }
}

</script>

<template>
    <div class="modal" ref="dialogElem">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" v-if="props.withHeader">
                    <h5 class="modal-title">
                        <slot name="title"></slot>
                    </h5>
                </div>
                <div class="modal-body">
                    <slot name="body"></slot>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" @click="onAbort">{{ t(props.cancelCaption)
                    }}</button>
                    <button type="button" class="btn btn-primary" @click="$emit('submit')">{{ t(props.submitCaption)
                    }}</button>
                </div>
            </div>
        </div>
    </div>
</template>