<script>
    import { onMount } from "svelte";

    // import { api } from "$lib/lararium/api";

    let videoElement = $state();

    onMount(async () => {
        // await loadVideoAsBlob();
        // console.log(videoElement);
        // let videoUrlApi = `/video/video/123`;

        // const response = await api.get(videoUrlApi);
        // let m3u8Url = await response.text();
        // console.log(m3u8Url);
        // videoElement.src = m3u8Url;

        videoElement.src = "/video/static/index.m3u8";
    });

    let vId = $state("");

    const findVideo = async () => {
        // await loadVideoAsBlob(vId);
    };
</script>

<div>
    <div class="video-player">
        <video muted
            style="width: 400px; height: 200px;"
            bind:this={videoElement}
            controls
            preload="metadata"
            class="video-element"
        >
            Ваш браузер не поддерживает видео.
        </video>

                <video
            style="width: 400px; height: 200px;"
            src="video/anpcg.mp4"
            controls
            preload="metadata"
            class="video-element"
        >
            Ваш браузер не поддерживает видео.
        </video>

        <input bind:value={vId} />
        <button onclick={findVideo}>findVideo</button>
        <p style="color: white;">{vId}</p>
    </div>
</div>

<style>
</style>
