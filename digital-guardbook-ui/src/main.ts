import "bootstrap/dist/css/bootstrap.css"

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import { ApolloClient, createHttpLink, InMemoryCache } from '@apollo/client/core'
import { DefaultApolloClient } from '@vue/apollo-composable'

/* import font awesome icon component */
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

import App from './App.vue'
import router from './router'
import i18n from './i18n/index'

const httpLink = createHttpLink({
  uri: '/api',
  fetch: (reqInfo: RequestInfo | URL, options: RequestInit | undefined) => {
    return fetch(reqInfo, options)
  },
})

const cache = new InMemoryCache()

// Create the apollo client
const apolloClient = new ApolloClient({
  link: httpLink,
  cache
})

const app = createApp(App)

app.provide(DefaultApolloClient, apolloClient)

app.component('font-awesome-icon', FontAwesomeIcon)
app.use(createPinia())
app.use(router)
app.use(i18n)

app.mount('#app')

import "bootstrap/dist/js/bootstrap.js"