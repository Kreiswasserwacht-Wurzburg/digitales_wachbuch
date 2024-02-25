<script setup lang="ts">
import { useThemeStore } from '@/store/theme';
import { ref, watch } from 'vue';
import { onMounted } from 'vue';


const theme = ref<string>()
const store = useThemeStore()

watch(theme, (newValue, oldValue) => {
    if (newValue) {
        if (oldValue) {
            store.setTheme(newValue)
        }

        document.documentElement.setAttribute('data-bs-theme', newValue)
    }
})

onMounted(() => theme.value = store.getTheme)

</script>
<template>
    <select class="form-select" v-model="theme" v-if="theme">
        <option selected value="auto">Auto</option>
        <option value="light">Light</option>
        <option value="dark">Dark</option>
    </select>
</template>