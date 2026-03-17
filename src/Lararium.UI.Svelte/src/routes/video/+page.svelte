<script>
    import { onMount } from "svelte";
    import { page } from "$app/stores";
    import { api } from "$lib/lararium/api";

    import { Video as VideoModel } from "$lib/models/Video";
    import VideoItem from "./_components/VideoItem.svelte";

    let loading = $state();
    let result = $state("");
    let videoList = $state([]);
    let error = $state("123");
    onMount(async () => {
        loading = true;
        try {
            let videoUrlApi = `/video/list`;

            const response = await api.get(videoUrlApi);

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }

            let json = await response.json();
            videoList = json.map((v) => new VideoModel(v));
        } catch (err) {
            error = {
                // Базовые типы
                raw: err,
                type: typeof err,
                constructorName: err?.constructor?.name,

                // Свойства объекта (многие ошибки не перечисляемы через Object.keys)
                keys: Object.keys(err || {}),
                json: JSON.stringify(err, Object.getOwnPropertyNames(err)), // Фикс: позволяет сериализовать стеки и сообщения

                // Текстовое описание
                string: err?.toString?.(),
                message: err?.message,
                stack: err?.stack,

                // Диагностика сети
                isNetworkError: err?.name === "TypeError",
                isOnline:
                    typeof navigator !== "undefined"
                        ? navigator.onLine
                        : "unknown",
            };
        }

        loading = false;
    });
</script>

<div class="videos">
    <ul>
        {#each Object.entries(error) as [key, value]}
            {#if value}
                <li><strong>{key}:</strong> {value}</li>
            {/if}
        {/each}
    </ul>
    <div class="videos">
        {#if loading}
            <div class="loading">Загрузка...</div>
        {:else}
            <div class="video-list">
                {#each videoList as video}
                    <VideoItem model={video} />
                {/each}
            </div>
        {/if}
    </div>
</div>

<style>
    .videos {
        padding: 1rem;
    }

    .video-list {
        display: grid;
        grid-template-columns: repeat(1, minmax(0, 1fr));
        gap: 1rem;
    }

    /* Адаптивные брейкпоинты */
    @media (min-width: 640px) {
        .video-list {
            grid-template-columns: repeat(2, minmax(0, 1fr));
        }
    }

    @media (min-width: 768px) {
        .video-list {
            grid-template-columns: repeat(2, minmax(0, 1fr));
        }
    }

    @media (min-width: 1200px) {
        .video-list {
            grid-template-columns: repeat(3, minmax(0, 1fr));
        }
    }

    @media (min-width: 1500px) {
        .video-list {
            grid-template-columns: repeat(4, minmax(0, 1fr));
        }
    }
</style>
