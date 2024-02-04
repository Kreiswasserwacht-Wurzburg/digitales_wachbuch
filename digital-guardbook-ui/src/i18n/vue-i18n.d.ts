/**
 * global type definitions
 * using the typescript interface, you can define the i18n resources that is type-safed!
 */

/**
 * you need to import the some interfaces
 */
import {
  DefineLocaleMessage,
  DefineDateTimeFormat,
  DefineNumberFormat
} from 'vue-i18n'

declare module 'vue-i18n' {
  // define the locale messages schema
  export interface DefineLocaleMessage {
    nav: {
      dashboard: string,
      staff: string,
      operation: string
    },
    dashboard: {
      weather: {
        title: string,
        gust: string,
        noWarnings: string,
        from: string
        until: string
      },
      controlCentre: string
    },
    logBook: {
      title: string,
      time: string,
      author: string,
      message: string
    },
    sentry: {
      organisation: string,
      supervisor: string,
      startTime: string,
      startAction: string,
      stopAction: string,
      registrationTime: string,
      activeGuards: string
    }
  }

  // define the number format schema
  export interface DefineNumberFormat {
    currency: {
      style: 'currency'
      currencyDisplay: 'symbol'
      currency: string
    },
    decimal: {
      style: 'decimal',
      minimumFractionDigits: number,
      maximumFractionDigits: number
    }
    percent: {
      style: 'percent',
      useGrouping: boolean
    }
  }

  // define the datetime format schema
  export interface DefineDateTimeFormat {
    short: {
      hour: 'numeric'
      minute: 'numeric'
      second: 'numeric'
      timeZoneName: 'short'
      timezone: string
    },
    shortDateTime: {
      year:'numeric',
      month: 'numeric',
      day: 'numeric',
      hour: 'numeric',
      minute: 'numeric',
      second: 'numeric',
      timeZoneName: 'short',
      timezone: string
    }
  }
}