<!-- src/routes/videoplayer/+page.svelte -->
<script>
    // ПРАВИЛЬНЫЙ ИМПОРТ для Svelte 4
    import { onMount } from "svelte";
    import { page } from "$app/stores";
    import { Video } from "$lib/models/Video";
    import Tagpanel from "./_components/TagPanel.svelte";
    import Videoplayer from "./_components/Videoplayer.svelte";
    
    $: params = $page.url.searchParams;
    $: {
        console.log("Page changed:", $page);
    }

    let loading = true;

    // load data
    onMount(async () => {
        loading = false;
    });

    let currentVideoData = {
        id: 0,
        name: "Поход в горы 2023",
        duration: "35 мин",
        quality: "480p",
        size: "1.2 GB",
        tags: ["Природа", "Путешествие", "Горы", "Отдых"],
        thumbnail: "images/0bf507d9-995b-0dbd-1efd-62aa384d90df.jpg",
        date: "15.06.2023",
    };

    // "https://images.unsplash.com/photo-1593359677879-a4bb92f829d1?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=500&q=80"

    let video = new Video(currentVideoData);
</script>

<div>
    {#if loading}
        <p>loading...</p>
        {#each params.keys() as key}
            <li>{key}</li>
        {/each}
    {:else}
        <Videoplayer model={video} />
    {/if}
</div>

<style>
</style>
