<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import Clock from '@/components/date/Clock.vue'
import StationLogo from '@/components/station/StationLogo.vue'
import ThemeSwitcher from '@/components/general/ThemeSwitcher.vue'
</script>

<template>
  <header class="p-0 m-0">
    <nav class="navbar navbar-expand-md bg-body-tertiary p-0 m-0">
      <div class="container-fluid">
        <router-link class="navbar-brand" to="/">
          <StationLogo />
          </router-link>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav"
          aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav">
            <li class="nav-item">
              <router-link class="nav-link active" to="/">{{ $t("nav.dashboard") }}</router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link disabled" to="/personal">{{ $t("nav.staff") }}</router-link>
            </li>
            <li class="nav-item disabled">
              <router-link class="nav-link disabled" to="/mission">{{ $t("nav.operation") }}</router-link>
            </li>
          </ul>
        </div>
        <div class="navbar-text px-2 mx-2" style="border-right: 1px solid #000;">
            <Clock class="fw-bold" id="clock"></Clock>
            <ThemeSwitcher />
        </div>
      </div>
    </nav>
  </header>

  <RouterView />
</template>

<style scoped>
header {
  line-height: 1.5;
  max-height: 100vh;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

nav {
  width: 100%;
  font-size: 12px;
  text-align: center;
  margin-top: 2rem;
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }

  nav {
    text-align: left;
    margin-left: -1rem;
    font-size: 1rem;

    padding: 1rem 0;
    margin-top: 1rem;
  }
}
</style>
