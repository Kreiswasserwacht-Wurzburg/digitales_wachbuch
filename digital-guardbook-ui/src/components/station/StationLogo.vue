<script setup lang="ts">

import { useStationStore } from '@/store/station';
import { storeToRefs } from 'pinia';
import { onMounted } from 'vue';

const store = useStationStore()
const { logo, loading } = storeToRefs(store)

onMounted(() => {
    store.fetch()
})

</script>

<template>
    <img :src="logo.toString()" alt="Logo Wasserwacht Bayern"
        class="d-inline-block align-top" height="75" />
</template>