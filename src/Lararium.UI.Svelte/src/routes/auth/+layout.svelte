<script>
    let { children } = $props();
</script>

<svelte:head>
    <title>Auth</title>
</svelte:head>

<div class="page-body">
    {@render children()}
</div>

<style global>
    * {
        box-sizing: border-box;
        padding: 0;
        margin: 0;
    }

    .page-body {
        font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto,
            sans-serif;
        background: linear-gradient(135deg, #0a0f1e 0%, #111827 100%);
        min-height: 100vh;
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 20px;
        color: var(--text-primary);
        line-height: 1.5;
    }
</style>
