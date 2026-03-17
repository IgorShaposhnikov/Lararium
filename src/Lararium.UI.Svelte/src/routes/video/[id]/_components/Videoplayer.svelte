<script>
    import { onMount } from "svelte";
    import { api } from "$lib/lararium/api";
    import TagPanel from "./TagPanel.svelte";
    import { page } from "$app/state";
    import { Video as VideoModel } from "$lib/models/Video";
    import { larariumVideo } from "$lib/lararium/video";


    let { model: videoData } = $props();

    // 1. Инициализируем переменные состояния
    let model = $state(videoData || null);
    let videoElement = $state(null); // Обязательно $state для bind:this

    let videoId = $derived(page.params.id);

    // Функция загрузки (исправлена переменная v)
    const loadVideoStream = async (id) => {
        let videoUrlApi = `/video/list`;
        const response = await api.get(videoUrlApi);
        if (!response.ok) throw new Error(`HTTP ${response.status}`);
        
        let json = await response.json();
        // Предполагаем, что нам нужно найти конкретное видео по id
        const data = json.find(v => v.id === id) || json[0]; 
        return new VideoModel(data);
    };

    // 2. Используем эффект для отслеживания готовности model и videoElement
    $effect(() => {
        if (model && videoElement) {
            setupVideo();
        }
    });

    async function setupVideo() {
        try {
            const src = await larariumVideo.loadVideoStream(model.id);
            if (videoElement) videoElement.src = src;
        } catch (e) {
            console.error("Ошибка загрузки видео:", e);
        }
    }

    onMount(async () => {
        if (!model && videoId) {
            model = await loadVideoStream(videoId);
        }
        console.log("Model loaded:", model);
    });
</script>

{#if model}
    <div class="container">
        <div class="main-content">
            <section class="video-main-block">
                <div class="video-info-top">
                    <div class="video-header">
                        <div class="video-title-section">
                            <h2 class="video-name">{model.title}</h2>
                            <div class="video-details">
                                <span><i class="far fa-clock"></i> {model.duration}</span>
                                <span><i class="fas fa-expand-alt"></i> {model.quality}</span>
                                <span><i class="far fa-hdd"></i> {model.size}</span>
                            </div>
                        </div>
                    </div>
                    <TagPanel tags={model.tags} />
                </div>

                <div class="video-container">
                    <div class="video-player">
                        <video
                            muted
                            autoplay
                            bind:this={videoElement} 
                            style="width: 100%; height: 100%;"
                            controls
                        >
                            <track kind="captions" />
                        </video>
                    </div>
                </div>
            </section>
        </div>
    </div>
{/if}

<style>
    @import "./Videoplayer.css";
</style>