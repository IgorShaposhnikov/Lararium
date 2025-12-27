<script>
    import TagPanel from "./TagPanel.svelte";

    let { model } = $props();
</script>

<div class="container">
    <div class="main-content">
        <!-- Объединенный блок названия и видеоплеера -->
        <section class="video-main-block">
            <!-- Информация о видео (выше видеоплеера) -->
            <div class="video-info-top">
                <div class="video-header">
                    <div class="video-title-section">
                        <h2 class="video-name" id="videoName">
                            {model.name}
                        </h2>
                        <div class="video-details">
                            <span id="videoDuration">
                                <i class="far fa-clock"></i>
                                {model.duration}
                            </span>
                            <span id="videoQuality">
                                <i class="fas fa-expand-alt"></i>
                                {model.quality}
                            </span>
                            <span id="videoSize">
                                <i class="far fa-hdd"></i>
                                {model.size}
                            </span>
                        </div>
                    </div>
                </div>

                <TagPanel tags={model.tags} />
            </div>

            <!-- Видеоплеер -->
            <div class="video-container">
                <div class="video-player" id="videoPlayer">
                    <!-- Здесь будет видео -->

                    <video style="width: 100%; height: 100%;" src={"video/vid_720p.mp4"} controls>
                        <track kind="captions" />
                    </video>
                    <!-- <div style="position: relative; width: 100%; height: 100%;">
                        <img alt="{model.name}" src={model.thumbnail} style="width: 100%; height: 100%; object-fit: cover; filter: brightness(0.7);" />
                        <div
                            style="position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); color: white; text-align: center;"
                        >
                            <i
                                class="fas fa-play-circle"
                                style="font-size: 80px; cursor: pointer; margin-bottom: 15px;"
                                ononclick={playVideo()}
                            ></i>
                            <p style="font-size: 22px; font-weight: 600;">
                                ${model.name}
                            </p>
                            <p style="font-size: 16px; margin-top: 5px;">
                                ${model.duration} • {model.quality}
                            </p>
                        </div>
                    </div> -->

                    <div
                        style="display: flex; justify-content: center; align-items: center; height: 100%; color: #94a3b8; flex-direction: column; gap: 15px;"
                    >
                        <i class="fas fa-play-circle" style="font-size: 80px;"
                        ></i>
                        <p style="font-size: 20px; text-align: center;">
                            Выберите видео из списка ниже для просмотра
                        </p>
                    </div>
                </div>
            </div>
        </section>

        <!-- Кнопки действий -->
        <div class="video-actions">
            <button class="action-btn download" ononclick={downloadVideo()}>
                <i class="fas fa-download"></i> Скачать
            </button>
            <button class="action-btn gallery" ononclick={openGallery()}>
                <i class="fas fa-images"></i> Галерея
            </button>
            <button class="action-btn edit" ononclick={editVideo()}>
                <i class="fas fa-edit"></i> Редактировать
            </button>
            <button class="action-btn delete" ononclick={deleteVideo()}>
                <i class="fas fa-trash"></i> Удалить
            </button>
        </div>

        <!-- Список видео -->
        <section class="videos-section">
            <div class="videos-header">
                <h2>Другие видео</h2>
            </div>
            <div class="videos-grid" id="videosGrid">
                <!-- Видео карточки будут добавляться динамически -->
            </div>
        </section>
    </div>
</div>

<style>
    @import "Videoplayer.css";
</style>
