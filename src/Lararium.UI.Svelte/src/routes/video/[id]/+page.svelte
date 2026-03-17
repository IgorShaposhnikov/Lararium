<!-- "https://images.unsplash.com/photo-1593359677879-a4bb92f829d1?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=500&q=80" -->
<!-- src/routes/video/[id]/+page.svelte -->
<script>
    import { onMount } from "svelte";
    import { page } from "$app/stores";

    import { Video } from "$lib/models/Video";

    import Tagpanel from "./_components/TagPanel.svelte";
    import Videoplayer from "./_components/Videoplayer.svelte";

    import { api } from "$lib/lararium/api";

    let videoId = $page.params.id;
    let videoModel = $state(null);


    onMount(async () => {
        await loadVideoInfo(videoId);
    });
    let json = $state();
    let loadVideoInfo = async (id) => {
        let videoUrlApi = `/video/${id}`;

        // не грузится на телефоне
        const response = await api.get(videoUrlApi);

        if (!response.ok) {
            json = response.status;
            throw new Error(`HTTP ${response.status}`);
        }

        json = await response.json();
        videoModel = new Video(json);
        console.log(json);
    };
</script>

<div>
{json}
    {#if videoModel}
        <Videoplayer model={videoModel} />
    {:else}
        <p>loading...</p>
    {/if}
</div>

<style>
</style>
