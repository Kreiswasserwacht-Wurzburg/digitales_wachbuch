<script setup lang="ts" generic="T extends {id: string}">
import { useI18n } from 'vue-i18n'
import { ref } from 'vue';
import { toRef } from 'vue';

const { t, n, d } = useI18n({
  useScope: 'global'
})

const props = defineProps({
  dataSource: {
    type: Array<T>,
    required: true
  },
  selection: {
    type: Array<T>,
    required: true
  },
  dragDrop: {
    type: Boolean,
    required: false,
    default: true
  },
  multiselect: {
    type: Boolean,
    required: false,
    default: true
  },
  search: {
    type: Boolean,
    required: false,
    default: true
  },
})

const selection = toRef(props, 'selection')
const dataSource = toRef(props, 'dataSource')

const emit = defineEmits<{
  "update:selection": [value: Array<T>]
}>()

const dragState = ref({
  source: {
    dragenter: false
  },
  target: {
    dragenter: false
  }
})

enum DragSource {
  Source,
  Selection
}

function startDrag(event: DragEvent, item: any, src: DragSource) {
  event.dataTransfer?.setData("key", item.id)
  event.dataTransfer?.setData("source", DragSource[src])

  switch (src) {
    case DragSource.Source:
      event.dataTransfer.dropEffect = "copy"
      break;
    case DragSource.Selection:
      event.dataTransfer.dropEffect = "move"
      break;
  }
}

function onDrop(event: DragEvent, target: DragSource) {
  const key = event.dataTransfer?.getData("key");
  const src: DragSource = DragSource[event.dataTransfer?.getData("source")]

  if (src === target) {
    return;
  }

  const item = dataSource.value.find((item) => item.id == key)

  if (item) {
    switch (target) {
      case DragSource.Selection:
        addItemToSelection(item)
        dragState.value.target.dragenter = false
        break;
      case DragSource.Source:
        removeItemFromSelection(item)
        dragState.value.source.dragenter = false
        break;
    }
  }

}

function addItemToSelection(item: T) {
  if (!itemAlreadySelected(item)) {
    selection.value.push(item)
    emit('update:selection', selection.value)
  }
}

function removeItemFromSelection(item: T) {
  if (selection.value) {
    const index = selection.value.findIndex((x) => x.id == item.id)
    if (index > -1) {
      selection.value.splice(index, 1)
      emit('update:selection', selection.value)
    }
  }
}

function itemAlreadySelected(item: T): boolean {
  if (selection.value) {
    return selection.value.find((x) => x.id == item.id)
  }
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
          <input type="text" class="form-control" :placeholder="t('common.search')" v-if="search" />
          <div class="dropzone h-100" :class="{ dragover: dragState.source.dragenter }"
            @dragenter="dragState.source.dragenter = true; $event.preventDefault()"
            @dragleave="dragState.source.dragenter = false; $event.preventDefault()" @dragover="$event.preventDefault()"
            @drop="onDrop($event, DragSource.Source)">
            <ul class="list-group">
              <li class="list-group-item" :class="{ 'disabled': itemAlreadySelected(item) }" v-for="item in dataSource"
                @dblclick="addItemToSelection(item)">
                <div :data-id="item.id" :draggable="dragDrop" @dragstart="startDrag($event, item, DragSource.Source)">
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
          @drop="onDrop($event, DragSource.Selection)">
          <ul class="list-group">
            <li class="list-group-item" v-for="item in selection" @dblclick="removeItemFromSelection(item)">
              <div :data-id="item.id" :draggable="dragDrop" @dragstart="startDrag($event, item, DragSource.Selection)">
                <slot name="item" v-bind="item"></slot>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>