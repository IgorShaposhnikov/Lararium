<script>
    import { auth } from '$lib/stores/auth'
</script>

{#if auth.isAuthenticated}
  <slot />
{:else if auth.isLoading}
  <div>Загрузка...</div>
{/if}