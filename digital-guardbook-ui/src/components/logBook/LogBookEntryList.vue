<script setup lang="ts">
import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { DateTime } from 'luxon';
import type { LogBookEntry } from './logBookEntry'

import { useI18n } from 'vue-i18n'

const {t,n,d} = useI18n({
  useScope: 'global'
})

const props = defineProps<{
    from: DateTime | string,
    to?: DateTime | string
}>();

const { result, loading } = useQuery(gql`
    query ($from: String!, $to: String) {
        logBookEntries(from: $from, to: $to) {
            time
            author
            message
        }
    }`, {
    from: props.from.toString()
})

function convertDateTimeToString(dt: string)
{
    return DateTime.fromISO(dt).toLocaleString(DateTime.DATETIME_SHORT)
}
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
            <tr v-for="entry in result?.logBookEntries">
                <th scope="row">{{ d(entry.time,'shortDateTime') }}</th>
                <td>{{ entry.author }}</td>
                <td>{{ entry.message }}</td>
            </tr>
        </tbody>
    </table>
</template>