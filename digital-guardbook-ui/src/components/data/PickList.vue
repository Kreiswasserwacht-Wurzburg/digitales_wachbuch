<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { ref } from 'vue';

const { t, n, d } = useI18n({
  useScope: 'global'
})

export interface Props {
  data: any[],
  selection: any[],
  dragDrop: {type: boolean, required: false},
  multiselect:  {type: boolean, required: false}
}

const props = withDefaults(defineProps<Props>(), {
  data: Array<any>,
  selection: Array<any>,
  dragDrop: true,
  multiselect: false
})

const emit = defineEmits<{
  "value:selection": [value: any[]]
}>()

const dragState = ref({
  source: {
    dragenter: false
  },
  target: {
    dragenter: false
  }
})

function startDrag(event: DragEvent, item: any) {
  event.dataTransfer.setData("key", item.id)
}

function onDrop(event: DragEvent) {
  const key = event.dataTransfer.getData("key");
  const item = props.data.find((item) => item.id == key)
  addItemToSelection(item)

  dragState.value.target.dragenter = false
}

function xor(lhs: boolean, rhs: boolean): boolean {
  return !(lhs == rhs);
}

function addItemToSelection(item: any) {
  if (!itemAlreadySelected(item)) {
    props.selection.push(item)
    emit("value:selection", props.selection)
  }
}

function removeItemFromSelection(item: any) {
  const index = props.selection.findIndex((x) => x.id == item.id)
  if (index > -1) {
    props.selection.splice(index, 1)
  }
}

function itemAlreadySelected(item: any): boolean {
  return props.selection.find((x) => x.id == item.id)
}
</script>

<style scoped>
.dragover {
  border: 1px dashed #ccc
}
</style>

<template>
  <div class="container-fluid">
    <div class="row">
      <div class="col">
        <div>
          <slot name="sourceTitle"></slot>
        </div>
        <div>
          <input type="text" class="form-control" :placeholder="t('common.search')" />
          <div class="dropzone h-100" :class="{ dragover: dragState.source.dragenter }"
            @dragenter="dragState.source.dragenter = true; $event.preventDefault()"
            @dragleave="dragState.source.dragenter = false; $event.preventDefault()" @dragover="$event.preventDefault()">
            <ul class="list-group">
              <li class="list-group-item" :class="{ 'active': item.selected, 'disabled': itemAlreadySelected(item) }" v-for="item in data"
                @dblclick="addItemToSelection(item)">
                <div :data-id="item.id" :draggable="dragDrop" @dragstart="startDrag($event, item)">
                  <slot name="item" v-bind="item"></slot>
                </div>
              </li>
            </ul>
          </div>
        </div>
      </div>
      <div class="col">
        <div>
          <slot name="targetTitle"></slot>
        </div>
        <div class="dropzone h-100" :class="{ dragover: dragState.target.dragenter }"
          @dragenter="dragState.target.dragenter = true; $event.preventDefault()"
          @dragleave="dragState.target.dragenter = false; $event.preventDefault()" @dragover="$event.preventDefault()"
          @drop="onDrop($event)">
          <ul class="list-group">
            <li class="list-group-item" v-for="item in selection" @dblclick="removeItemFromSelection(item)">
              <div :data-id="item.id" :draggable="dragDrop">
                <slot name="item" v-bind="item"></slot>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>