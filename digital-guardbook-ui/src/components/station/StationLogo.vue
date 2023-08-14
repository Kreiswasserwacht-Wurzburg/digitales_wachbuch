<script setup lang="ts">
import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { ref, watch } from 'vue'

const { result } = useQuery(gql`
    query {
        station {
            name
        }
    }
`)

const url = ref('')

watch(result, value => {
    url.value = `https://meine.wasserwacht.bayern/logogenerator/H.php?kv=${result?.value?.station?.name}&ov=`
})
</script>

<template>
    <img :src="url" alt="Logo Wasserwacht Bayern"
        class="d-inline-block align-top" height="75" />
</template>