<script>
    import { page } from '$app/state';
    import { onMount } from 'svelte';
    import { goto } from "$app/navigation";

    function toReturnUrl() {
        goto(page.url.searchParams.get('returnUrl'));
    };
</script>

<ul>
    <button onclick={toReturnUrl}>To return url</button>
    {#each page.url.searchParams as [key, value]}
        <li><strong>{key}:</strong> {value}</li>
    {/each}
</ul>