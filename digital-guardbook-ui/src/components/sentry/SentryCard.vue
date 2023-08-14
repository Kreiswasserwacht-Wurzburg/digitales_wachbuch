<script setup lang="ts">
import { ref } from 'vue';
import SentryDisplay from './SentryDisplay.vue';
import SentryStart from './SentryStart.vue';

const sentry = ref(null)

</script>

<template>
    <div class="card">
        <div class="card-body">
            <div class="card-body">
                <SentryDisplay v-if="sentry != null" />
                <SentryStart v-else />
            </div>
        </div>
    </div>
</template>