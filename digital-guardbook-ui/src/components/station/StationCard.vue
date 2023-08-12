<script setup lang="ts">
import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { onMounted, ref } from 'vue'

const { result } = useQuery(gql`
    query {
        station {
            name
            address {
                street
                city
                zipCode
            }
        }
    }
`)

const station = ref({
    name: "Lorem"
})
</script>

<template>
    <div class="card">
        <div class="card-body">
            <h3 class="card-title">{{ result?.station?.name }}</h3>
            <div>{{ result?.station?.address?.street }}</div>
            <div>{{ result?.station?.address?.zipCode }} {{ result?.station?.address?.city }}</div>
            <!-- <br />
            <div>Telefon: @station?.PhoneNumber</div> -->
        </div>
    </div>
</template>

