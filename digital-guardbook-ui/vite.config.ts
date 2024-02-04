import { fileURLToPath, URL } from 'node:url'
import VueI18nPlugin from '@intlify/unplugin-vue-i18n/vite'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import path from 'node:path'

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    proxy: {
      "/weather": {
        target: "https://app-prod-ws.warnwetter.de/v30",
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path.replace(/^\/weather/, ""),
      },
      "/api": {
        target: "http://localhost:5282/graphql",
        changeOrigin: true,
        secure: false,
        rewrite: (path) => path.replace(/^\/api/, ""),
      },
    },
  },
  plugins: [
    vue(),
    VueI18nPlugin({
      include: path.resolve(__dirname, "src/i18n/locales/**")
    })
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  }
})
