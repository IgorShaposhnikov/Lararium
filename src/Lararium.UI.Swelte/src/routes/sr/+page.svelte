<script>
    // import { onMount } from "svelte";
    // import { auth, parseJwt, isTokenExpired } from "$lib/lararium/store/auth";
    // import { browser } from "$app/environment";
    import WeatherViewer from './_components/WeatherViewer.svelte';

    // let token = $state("");
    // onMount(() => {
    //     if (browser) {
    //         token = localStorage.getItem("access_token");
    //         console.log(token);
    //     }
    // });
</script>

<div>
<WeatherViewer />
    <!-- {#if token}
        {token}
        <br />
        Is token expired: {isTokenExpired()}

        <WeatherViewer />
    {:else}
        Unauthrized
    {/if} -->
</div>

<style>
</style>
