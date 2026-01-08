<script>
    import favicon from "$lib/assets/favicon.svg";

    let { children } = $props();
    export const ssr = false;
</script>

<svelte:head>
    <link rel="icon" href={favicon} />
    <link rel="stylesheet" href="/assets/fontawesome/7.0.0/css/all.min.css" />
</svelte:head>

{@render children()}

<style>
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
</style>
