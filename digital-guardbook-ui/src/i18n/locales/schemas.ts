/**
 * define the resource schema
 */

import en from './en.json'

// define message schema as master mesage schema
export type MessageSchema = typeof en

// define number format schema
export type NumberSchema = {
  currency: {
    style: 'currency'
    currencyDisplay: 'symbol'
    currency: string
  },
  decimal: {
    style: 'decimal', 
    minimumFractionDigits: 2, 
    maximumFractionDigits: 2
  },
  percent: {
    style: 'percent', 
    useGrouping: false
  }
}