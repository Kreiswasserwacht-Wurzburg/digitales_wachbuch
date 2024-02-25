<script setup lang="ts">
import { useStationStore } from '@/store/station';
import { storeToRefs } from 'pinia';
import { onMounted } from 'vue';

const store = useStationStore()
const { station, loading } = storeToRefs(store)

onMounted(() => {
    store.fetch()
})
</script>

<template>
    <div class="card">
        <div class="card-body" v-if="!loading && station">
            <h3 class="card-title">{{ station.name }}</h3>
            <div>{{ station.address?.street }}</div>
            <div>{{ station.address?.zipCode }} {{ station.address?.city }}</div>
            <!-- <br />
            <div>Telefon: @station?.PhoneNumber</div> -->
        </div>
    </div>
</template>
