<script setup lang="ts">
import { computed } from 'vue';

/* import the fontawesome core */
import { library } from '@fortawesome/fontawesome-svg-core'
import { faCloud, faSnowflake, faSun, faMoon, faCloudSun, faCloudSunRain, faCloudShowersHeavy, faCloudRain, faCloudMoonRain, faCloudMoon, faCloudBolt, faSmog, faCloudMeatball } from '@fortawesome/free-solid-svg-icons'

library.add(faCloud, faSnowflake, faSun, faMoon, faCloudSun, faCloudSunRain, faCloudShowersHeavy, faCloudRain, faCloudMoonRain, faCloudMoon, faCloudBolt, faSmog, faCloudMeatball)

const props = defineProps({
    iconNumber: {
        type: Number,
        required: true
    },
    isDay: {
        type: Boolean,
        required: true,
        default: true
    }
})

const iconClass = computed(() => {
    switch (props.iconNumber) {
        case 1:
        case 24:
        case 25:
            if (props.isDay) {
                return 'sun'
            } else {
                return 'moon'
            }
        case 2:
        case 3:
            if (props.isDay) {
                return 'cloud-sun'
            } else {
                return 'cloud-moon'
            }
        case 4:
        case 17:
            return 'cloud'
        case 5:
        case 6:
            return 'smog'
        case 7:
        case 8:
        case 10:
            return 'cloud-rain'
        case 9:
        case 11:
            return 'cloud-showers-heavy'
        case 12:
        case 13:
            return 'cloud-meatball'
        case 14:
        case 15:
        case 16:
            return 'snowflake'
        case 18:
        case 19:
        case 20:
        case 21:
        case 22:
        case 23:
            if (props.isDay) {
                return 'cloud-sun-rain'
            } else {
                return 'cloud-moon-rain'
            }
        case 26:
        case 27:
        case 28:
        case 29:
        case 30:
            return 'cloud-bolt'
    }

    return ''
});

</script>

<template>
    <font-awesome-icon :icon="['fas', iconClass]" />
</template>