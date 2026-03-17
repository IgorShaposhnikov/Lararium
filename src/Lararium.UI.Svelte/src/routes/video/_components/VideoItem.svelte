<script>
    let { model, showAuthor, showViews, isNew } = $props();
</script>

<a href="/video/{model.id}" class="video-card">
    <div class="video-preview">
        <div class="video-preview-inner">
            {#if isNew}
                <!-- Бейджи -->
                <div class="badge badge-new" title="Recently uploaded">
                    <i class="fas fa-star"></i>
                    <span>NEW</span>
                </div>
            {/if}

            <div class="badge badge-quality">FHD</div>

            <!-- Длительность -->
            <div class="badge badge-duration">6:17</div>

            <!-- Кнопка воспроизведения -->
            <div class="play-overlay">
                <div class="play-button">
                    <i class="fas fa-play"></i>
                </div>
            </div>

            <!-- Миниатюра -->
            <div style="width: 100%; height: 100%; position: relative;">
                <img
                    src="https://images.unsplash.com/photo-1593359677879-a4bb92f829d1?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=800&q=80"
                    alt="Make It Work"
                    class="video-thumbnail"
                    loading="lazy"
                />
            </div>
        </div>
    </div>

    <!-- Контент карточки -->
    <div class="video-content">
        <!-- Заголовок и аспект-рацио -->
        <div class="video-header">
            <h3 class="video-title">
                {model.title}
            </h3>
            <!-- Бейдж 16:9 вместо рейтинга -->
            <div class="badge-aspect-ratio" title="Aspect ratio: 16:9">
                <i class="fas fa-expand-alt"></i>
                <span>16:9</span>
            </div>
        </div>

        {#if showAuthor}
            <!-- Автор -->
            <div class="video-author">
                <div class="author-info">
                    <div class="author-avatar">A</div>
                    <span class="author-name">AuthorName123456789</span>
                </div>
            </div>
        {/if}

        <!-- Статистика -->
        <div class="video-stats">
            <div class="stats-left">
                {#if showViews}
                    <div class="stat-item">
                        <i class="fas fa-eye stat-icon"></i>
                        <span>1.2K</span>
                    </div>
                {/if}
                <div class="stat-item">
                    <i class="far fa-clock stat-icon"></i>
                    <span>1h ago</span>
                </div>
            </div>
        </div>

        <!-- Теги -->
        <div class="video-tags">
            <button class="tag" title="Search for: #Tag2"> Tag1 </button>
            <button class="tag" title="Search for: #Tag1"> Tag2 </button>
            <button class="tag-more" title="Show all 4 tags"> +2 </button>
        </div>

        <!-- Мета-теги -->
        <div class="video-meta-tags">
            <button class="meta-tag" title="Search for: Make It Work">
                <i class="fa-regular fa-user meta-tag-icon"></i>
                <span>Name</span>
            </button>
        </div>
    </div>
</a>

<style>
    /* ===== КАРТОЧКА ВИДЕО ===== */
    .video-card {
        display: block;
        text-decoration: none;
        background: linear-gradient(135deg, #1a1a1a 0%, #151515 100%);
        border-radius: 12px;
        border: 1px solid #2a2a2a;
        overflow: hidden;
        transition: all 0.3s ease;
        cursor: pointer;
        width: 100%;
    }

    .video-card:hover {
        transform: translateY(-4px);
        border-color: rgba(59, 130, 246, 0.4); /* Синий вместо оранжевого */
        box-shadow:
            0 20px 25px -5px rgba(59, 130, 246, 0.05),
            0 8px 10px -6px rgba(59, 130, 246, 0.05);
    }

    /* ===== ПРЕВЬЮ ВИДЕО ===== */
    .video-preview {
        position: relative;
        background: #0f0f0f;
        overflow: hidden;
        aspect-ratio: 16 / 9;
    }

    /* Бэкап для старых браузеров */
    .video-preview::before {
        content: "";
        display: block;
        padding-bottom: 56.25%; /* 16:9 aspect ratio */
    }

    .video-preview-inner {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
    }

    /* Миниатюра видео */
    .video-thumbnail {
        width: 100%;
        height: 100%;
        object-fit: contain;
        transition: transform 0.3s ease;
    }

    .video-card:hover .video-thumbnail {
        transform: scale(1.05);
    }

    /* ===== БЕЙДЖИ НА ИЗОБРАЖЕНИИ ===== */
    .badge {
        position: absolute;
        z-index: 20;
        padding: 4px 10px;
        border-radius: 8px;
        font-size: 11px;
        font-weight: bold;
        display: flex;
        align-items: center;
        gap: 6px;
        backdrop-filter: blur(4px);
        border: 1px solid;
        box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
    }

    .badge-new {
        top: 12px;
        left: 12px;
        background: linear-gradient(135deg, var(--primary) 0%, var(--secondary) 100%); /* Синий вместо оранжевого */
        border-color: rgba(37, 99, 235, 0.5);
        color: white;
    }
    
    .badge-quality {
        top: 12px;
        right: 12px;
        background: linear-gradient(135deg, var(--primary) 0%, var(--secondary) 100%); /* Синий градиент */
        border-color: rgba(96, 165, 250, 0.5);
        color: white;
    }

    .badge-aspect {
        top: 48px;
        right: 12px;
        background: linear-gradient(
            to right,
            rgba(71, 85, 105, 0.95),
            rgba(30, 41, 59, 0.95)
        );
        border-color: rgba(100, 116, 139, 0.5);
        color: #e2e8f0;
    }

    .badge-duration {
        bottom: 12px;
        right: 12px;
        background: rgba(0, 0, 0, 0.9);
        border: 1px solid rgba(255, 255, 255, 0.1);
        color: white;
        font-size: 12px;
        padding: 4px 10px;
    }

    /* Бейдж 16:9 вместо рейтинга */
    .badge-aspect-ratio {
        display: inline-flex;
        align-items: center;
        gap: 4px;
        padding: 5px 9px;
        border-radius: 9999px;
        font-size: 13px;
        font-weight: 600;
        background: linear-gradient(
            135deg,
            rgba(59, 130, 246, 0.2),
            rgba(37, 99, 235, 0.2)
        );
        border: 1px solid rgba(59, 130, 246, 0.4);
        color: #93c5fd; /* Синий текст */
        flex-shrink: 0;
        line-height: 1;
    }

    .badge-aspect-ratio i {
        font-size: 11px;
    }

    /* ===== КНОПКА ВОСПРОИЗВЕДЕНИЯ ===== */
    .play-overlay {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        display: flex;
        align-items: center;
        justify-content: center;
        background: rgba(0, 0, 0, 0.2);
        opacity: 0;
        transition: all 0.3s ease;
        z-index: 15;
    }

    .video-card:hover .play-overlay {
        opacity: 1;
    }

    .play-button {
        width: 80px;
        height: 80px;
        border-radius: 50%;
        background: linear-gradient(135deg, var(--primary) 0%, var(--secondary) 100%); /* Синий вместо оранжевого */
        border: 1px solid rgba(96, 165, 250, 0.3); /* Синяя граница */
        display: flex;
        align-items: center;
        justify-content: center;
        transition: all 0.3s ease;
        box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
    }

    .video-card:hover .play-button {
        transform: scale(1.1);
        background: linear-gradient(135deg, var(--primary) 0%, var(--secondary) 100%) /* Синий при ховере */
    }

    .play-button i {
        color: white;
        font-size: 32px;
        margin-left: 4px;
    }

    /* ===== КОНТЕНТ КАРТОЧКИ ===== */
    .video-content {
        padding: 16px;
    }

    /* Заголовок и аспект-рацио */
    .video-header {
        display: flex;
        align-items: flex-start;
        justify-content: space-between;
        gap: 8px;
        margin-bottom: 12px;
    }

    .video-title {
        font-size: 16px;
        font-weight: 600;
        color: white;
        line-height: 20px;
        margin: 0;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
        overflow: hidden;
        text-overflow: ellipsis;
        flex: 1;
        transition: color 0.3s ease;
    }

    .video-card:hover .video-title {
        color: #60a5fa; /* Синий вместо оранжевого при ховере */
    }

    /* Автор видео */
    .video-author {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 8px;
        margin-bottom: 12px;
    }

    .author-info {
        display: flex;
        align-items: center;
        gap: 10px;
        color: #9ca3af;
        font-size: 14px;
        transition: color 0.3s ease;
        flex-shrink: 1;
        min-width: 0;
        cursor: pointer;
    }

    .author-info:hover {
        color: #60a5fa; /* Синий вместо оранжевого */
    }

    .author-avatar {
        width: 24px;
        height: 24px;
        border-radius: 50%;
        background: linear-gradient(135deg, var(--primary) 0%, var(--secondary) 100%);
        display: flex;
        align-items: center;
        justify-content: center;
        color: white;
        font-size: 11px;
        font-weight: bold;
        flex-shrink: 0;
    }

    .author-name {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        transition: color 0.3s ease;
    }

    /* Статистика */
    .video-stats {
        display: flex;
        align-items: center;
        justify-content: space-between;
        font-size: 12px;
        margin-bottom: 12px;
    }

    .stats-left {
        display: flex;
        align-items: center;
        gap: 12px;
        color: #6b7280;
    }

    .stat-item {
        display: flex;
        align-items: center;
        gap: 4px;
    }

    .stat-icon {
        font-size: 12px;
        color: #9ca3af;
    }

    /* Теги */
    .video-tags {
        display: flex;
        align-items: center;
        gap: 6px;
        flex-wrap: wrap;
        margin-bottom: 12px;
    }

    .tag {
        padding: 0.25rem 0.625rem;
        background: linear-gradient(
            to right,
            rgba(59, 130, 246, 0.15),
            rgba(37, 99, 235, 0.15)
        ); /* Синий градиент */
        color: #93c5fd; /* Синий текст */
        border-radius: 0.5rem;
        font-size: 0.75rem;
        font-weight: 600;
        border: 1px solid rgba(59, 130, 246, 0.3); /* Синяя граница */
        transition: all 150ms cubic-bezier(0.4, 0, 0.2, 1);
        box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
        max-width: 120px;
        flex-shrink: 0;
        cursor: pointer;
    }

    .tag:hover {
        background: linear-gradient(
            to right,
            rgba(59, 130, 246, 0.25),
            rgba(37, 99, 235, 0.25)
        );
        border-color: rgba(59, 130, 246, 0.5);
    }

    .tag-more {
        color: #9ca3af;
        font-size: 11px;
        font-weight: 600;
        padding: 4px 8px;
        background: rgba(55, 65, 81, 0.3);
        border: 1px solid rgba(55, 65, 81, 0.3);
        border-radius: 8px;
        cursor: pointer;
        transition: all 0.3s ease;
        border: none;
        font-family: inherit;
    }

    .tag-more:hover {
        color: #93c5fd; /* Синий вместо оранжевого */
        background: rgba(59, 130, 246, 0.2);
        border-color: rgba(59, 130, 246, 0.4);
    }

    /* Мета-теги */
    .video-meta-tags {
        display: flex;
        align-items: center;
        gap: 6px;
        flex-wrap: wrap;
    }

    .meta-tag {
        display: inline-flex;
        align-items: center;
        gap: 4px;
        padding: 2px 8px;
        background: linear-gradient(
            to right,
            rgba(34, 197, 94, 0.2),
            rgba(16, 185, 129, 0.2)
        );
        border: 1px solid rgba(74, 222, 128, 0.4);
        color: #86efac;
        border-radius: 6px;
        font-size: 11px;
        font-weight: 600;
        max-width: 300px;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        cursor: pointer;
        transition: all 0.3s ease;
        font-family: inherit;
        box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
    }

    .meta-tag:hover {
        background: linear-gradient(
            to right,
            rgba(34, 197, 94, 0.3),
            rgba(16, 185, 129, 0.3)
        );
        border-color: rgba(74, 222, 128, 0.6);
    }

    .meta-tag-icon {
        font-size: 11px;
    }

    .meta-tag-warning {
        display: inline-flex;
        align-items: center;
        gap: 4px;
        padding: 2px 8px;
        background: linear-gradient(
            to right,
            rgba(59, 130, 246, 0.2),
            rgba(37, 99, 235, 0.2)
        ); /* Синий градиент */
        border: 1px solid rgba(96, 165, 250, 0.4); /* Синяя граница */
        color: #93c5fd; /* Синий текст */
        border-radius: 6px;
        font-size: 11px;
        font-weight: 600;
        box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
    }

    .meta-tag-warning-icon {
        font-size: 11px;
    }
</style>
