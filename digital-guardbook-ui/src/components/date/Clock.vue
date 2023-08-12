<script lang="ts">
import { computed, defineComponent, getCurrentInstance } from 'vue'
import { DateTime } from 'luxon'

const delay = ms => new Promise(res => setTimeout(res, ms));

export default defineComponent({
    data() {
        return {
            dateTimeStr: DateTime.now().toLocaleString(DateTime.DATETIME_SHORT_WITH_SECONDS)
        }
    },
    methods: {
        async setTime() {
            await delay(500);
            this.dateTimeStr = DateTime.now().toLocaleString(DateTime.DATETIME_SHORT_WITH_SECONDS);
            this.setTime();
        }
    },
    mounted() {
        this.setTime();
    }
})

</script>

<template>
    <span>{{ this.dateTimeStr }}</span>
</template>