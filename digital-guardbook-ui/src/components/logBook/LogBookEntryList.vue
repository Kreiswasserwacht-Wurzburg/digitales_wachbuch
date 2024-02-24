<script setup lang="ts">
import { DateTime } from 'luxon';
import { useLogBookStore } from '@/store/logBook'
import { storeToRefs } from 'pinia'

import { useI18n } from 'vue-i18n'
import { onMounted } from 'vue';
import { vModelCheckbox } from 'vue';

const store = useLogBookStore()

const { logBookEntries, loading, socket} = storeToRefs(store)

// const socket = new WebSocket("ws://localhost:5282/ws");

const props = defineProps<{
    from: DateTime | string,
    to?: DateTime | string
}>();


// socket.onmessage = function(event){
//     console.log("Web Socket Message!");
//     store.fetchByTime(props.from, props.to);
// }

const {t,n,d} = useI18n({
  useScope: 'global'
})


onMounted(() => {
    store.fetchByTime(props.from, props.to)
})

</script>

<template>
    <h2>{{t('logBook.title')}}</h2>
    <table class="table table-striped">
        <thead>
            <tr>
                <th scope="col">{{t('logBook.time')}}</th>
                <th scope="col">{{t('logBook.author')}}</th>
                <th scope="col">{{t('logBook.message')}}</th>
            </tr>
        </thead>
        <tbody v-if="!loading"  class="table-group-divider">
            <tr v-for="entry in logBookEntries">
            <tr v-for="entry in logBookEntries">
                <th scope="row">{{ d(entry.time,'shortDateTime') }}</th>
                <td>{{ entry.author }}</td>
                <td>{{ entry.message }}</td>
            </tr>
        </tbody>
    </table>
</template>