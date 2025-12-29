<script>
    import { onMount } from "svelte";
    import { browser } from "$app/environment";
    import { api } from "$lib/lararium/api";

    let weathers = $state([]);

    onMount(async () => {
        const res = await api.get("/Weather");
        const resJson = await res.json();
        weathers = resJson.map((x) => new Weather(x));
    });

    class Weather {
        date = null;
        temperatureC = null;
        temperatureF = null;
        summary = null;
        constructor(data) {
            Object.assign(this, data);
        }
    }
</script>

{#if weathers.length === 0}
    <p>Loading...</p>
{:else}
    <ul>
        {#each weathers as w}
            <li>
                {w.data} | {w.temperatureC} | {w.temperatureF} | {w.summary}
            </li>
        {/each}
    </ul>
{/if}