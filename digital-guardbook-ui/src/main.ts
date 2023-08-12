import "bootstrap/dist/css/bootstrap.css"

import { createApp} from 'vue'

import { ApolloClient, createHttpLink, InMemoryCache } from '@apollo/client/core'
import { DefaultApolloClient } from '@vue/apollo-composable'
import App from './App.vue'
import router from './router'

const httpLink = createHttpLink({
  uri: '/api',
  fetch: (uri: RequestInfo, options: RequestInit) => {
    return fetch(uri, options)
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


app.use(router)

app.mount('#app')

import "bootstrap/dist/js/bootstrap.js"