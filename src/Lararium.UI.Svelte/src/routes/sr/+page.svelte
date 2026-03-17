<script>
    import { onMount } from "svelte";

    import { api } from "$lib/lararium/api";

    let videoElement = $state();

    onMount(async () => {
        // let videoUrlApi = `/video/video/019bdc84-cfaa-7d09-bf97-1560cb24efa5`;

        // const response = await api.get(videoUrlApi);

        // if (!response.ok) {
        //     throw new Error(`HTTP ${response.status}`);
        // }

        // const blob = await response.blob();
        // videoElement.src = URL.createObjectURL(blob);
        // videoElement.src = "videos\\test\\playlist.m3u8";
    });
</script>

<div>
    <div class="video-player">
        <video
            muted
            bind:this={videoElement}
            style="width: 400px; height: 200px;"
            controls
            src="videos\\test\\master.m3u8"
            preload="metadata"
            class="video-element"
        >
            Ваш браузер не поддерживает видео.
        </video>
    </div>
</div>

<style>
</style>
