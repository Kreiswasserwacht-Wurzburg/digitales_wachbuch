import { defineStore } from 'pinia'
import { computed } from 'vue'

export const useThemeStore = defineStore('theme', () => {
    let _theme: string

    const getTheme = computed(() => {
        if (_theme) {
            return _theme
        }

        _theme = getPreferredTheme()

        return _theme
    })

    function getStoredTheme(): string | null {
        return localStorage.getItem('theme')
    }

    function getPreferredTheme(): string {
        const storedTheme = getStoredTheme()

        if (storedTheme) {
            return storedTheme
        }

        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
    }

    function setTheme(theme: string) {
        if (theme === 'auto') {
            _theme = window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
            localStorage.removeItem('theme')
        } else {
            _theme = theme
            localStorage.setItem('theme', _theme)
        }
    }

    return { getTheme, setTheme }
})