import { createI18n, type I18nOptions } from "vue-i18n";
import en from './locales/en.json'
import deDE from './locales/deDE.json'


export default createI18n<false>({
    legacy: false,
    locale: "de-DE",
    fallbackLocale: "en",
    globalInjection: true,
    messages: {
        'en': en,
        'de-DE': deDE
    },
    datetimeFormats: {
        "en": {
            short: {
                hour: "numeric",
                minute: "numeric",
                second: "numeric",
                timezone: "Europe/London",
                timeZoneName: "short"
            }
        },
        "de-DE": {
            short: {
                hour: "numeric",
                minute: "numeric",
                second: "numeric",
                timezone: "Europe/Berlin",
                timeZoneName: "short"
            }
        }
    },
    numberFormats: {
        'en': {
            currency: {
                currency: "EUR",
                currencyDisplay: "symbol",
                style: "currency"
            },
            decimal: {
                maximumFractionDigits: 2,
                minimumFractionDigits: 2,
                style: "decimal"
            },
            percent: {
                style: "percent",
                useGrouping: false
            }
        },
        'de-DE': {
            currency: {
                currency: "EUR",
                currencyDisplay: "symbol",
                style: "currency"
            },
            decimal: {
                maximumFractionDigits: 2,
                minimumFractionDigits: 2,
                style: "decimal"
            },
            percent: {
                style: "percent",
                useGrouping: false
            }
        }
    },
}
)