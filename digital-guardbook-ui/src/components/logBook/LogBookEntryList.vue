<script setup lang="ts">
import { DateTime } from 'luxon';
import { useLogBookStore } from '@/store/logBook'
import { storeToRefs } from 'pinia'

import { useI18n } from 'vue-i18n'
import { onMounted } from 'vue';
import type { LogBookEntry } from '../../models/logBook';

const { t, n, d } = useI18n({
    useScope: 'global'
})

const store = useLogBookStore()

const { logBookEntries, loading } = storeToRefs(store)

const props = defineProps<{
    from: DateTime,
    to?: DateTime
}>();

onMounted(() => {
    store.fetchByTime(props.from, props.to)
})

// function formatDate(entry: LogBookEntry): string {
//     let dt: Date;
//     if (typeof (entry.time) == typeof (DateTime)) {
//         dt = entry.time.toJSDate()
//     }
//     else if (typeof (entry.time) == typeof ("")) {
//         dt = DateTime.fromISO(entry.time as unknown as string).toJSDate()
//     }
//     else {
//         return ""
//     }

//     return d(dt, 'shortDateTime')
// }

</script>

<template>
    <h2>{{ t('logBook.title') }}</h2>
    <table class="table table-striped">
        <thead>
            <tr>
                <th scope="col">{{ t('logBook.time') }}</th>
                <th scope="col">{{ t('logBook.author') }}</th>
                <th scope="col">{{ t('logBook.message') }}</th>
            </tr>
        </thead>
        <tbody v-if="!loading" class="table-group-divider">
            <tr v-for="entry in logBookEntries">
                <!-- <th scope="row">{{ formatDate(entry) }}</th> -->
                <th scope="row">{{ d(entry.time.toLocaleString(),'shortDateTime') }}</th>
                <td>{{ entry.author }}</td>
                <td>{{ entry.message }}</td>
            </tr>
        </tbody>
    </table>
</template>