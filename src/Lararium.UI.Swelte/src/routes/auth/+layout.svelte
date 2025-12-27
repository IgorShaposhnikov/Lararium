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
    :root {
        --primary: #2563eb;
        --primary-dark: #1d4ed8;
        --primary-light: #60a5fa;
        --secondary: #7c3aed;
        --accent: #db2777;
        --bg-dark: #0a0f1e;
        --bg-card: #111827;
        --bg-card-light: #1f2937;
        --text-primary: #f3f4f6;
        --text-secondary: #d1d5db;
        --text-muted: #9ca3af;
        --border-color: #374151;
        --border-light: #4b5563;
        --success: #059669;
        --warning: #d97706;
        --danger: #dc2626;
        --border-radius: 12px;
        --shadow: 0 5px 20px rgba(0, 0, 0, 0.5);
        --shadow-hover: 0 8px 25px rgba(0, 0, 0, 0.6);
        --transition: all 0.3s ease;
    }

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
