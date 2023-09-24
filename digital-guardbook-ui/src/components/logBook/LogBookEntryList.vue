<script setup lang="ts">
import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { DateTime } from 'luxon';
import type { LogBookEntry } from './logBookEntry'

const props = defineProps<{
    from: DateTime | string,
    to: {
        type: DateTime | string,
        required: false
    }
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

function convertDateTimeToString(dt: String)
{
    return DateTime.fromISO(dt).toLocaleString(DateTime.DATETIME_SHORT)
}
</script>

<template>
    <h2>Log-Buch</h2>
    <table class="table table-striped">
        <thead>
            <tr>
                <th scope="col">Zeit</th>
                <th scope="col">Autor</th>
                <th scope="col">Text</th>
            </tr>
        </thead>
        <tbody v-if="!loading"  class="table-group-divider">
            <tr v-for="entry in result?.logBookEntries">
                <th scope="row">{{ convertDateTimeToString(entry.time) }}</th>
                <td>{{ entry.author }}</td>
                <td>{{ entry.message }}</td>
            </tr>
        </tbody>
    </table>
</template>