import { DateTime } from "luxon";
import { defineStore } from "pinia";
import { ref } from "vue";

export const curSentryStore = defineStore("curSentryStore", {
  state: () => ({
    from: ref<DateTime>(),
    to: ref<DateTime>(),
  }),
});