import "bootstrap/dist/css/bootstrap.css"

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import apolloClient from '@/plugins/apollo'
import { DefaultApolloClient } from '@vue/apollo-composable'

/* import font awesome icon component */
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

import App from './App.vue'
import router from './router'
import i18n from './i18n/index'

const app = createApp(App)

app.provide(DefaultApolloClient, apolloClient)

app.component('font-awesome-icon', FontAwesomeIcon)
app.use(createPinia())
app.use(router)
app.use(i18n)

app.mount('#app')

app.config.globalProperties.window = window

import "bootstrap/dist/js/bootstrap.js"