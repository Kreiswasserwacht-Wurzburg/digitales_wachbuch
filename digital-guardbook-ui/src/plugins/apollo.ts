
import { ApolloClient, createHttpLink, InMemoryCache } from '@apollo/client/core'

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

export default apolloClient