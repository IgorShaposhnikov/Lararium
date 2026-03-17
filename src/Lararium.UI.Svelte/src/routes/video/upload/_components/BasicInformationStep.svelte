<script>
    let { title = $bindable(), summary = $bindable() } = $props();
</script>

<div class="step-content active" id="step1">
    <div class="step-header">
        <h2 class="step-title-main">Основная информация</h2>
        <p class="step-subtitle">Введите основную информацию о вашем видео</p>
    </div>

    <form class="upload-form">
        <div class="form-group">
            <label class="form-label" for="videoTitle">
                Название видео *
            </label>
            <div class="input-with-icon">
                <i class="fas fa-heading input-icon"></i>
                <input
                    class="form-input"
                    type="text"
                    id="videoTitle"
                    placeholder="Введите название видео"
                    required=""
                    bind:value={title}
                />
            </div>
        </div>

        <div class="form-group">
            <label class="form-label" for="videoDescription">
                Описание <span class="optional">(необязательно)</span>
            </label>
            <textarea
                class="form-textarea"
                id="videoDescription"
                placeholder="Опишите ваше видео..."
                bind:value={summary}
            ></textarea>
        </div>

        <!-- <div class="form-group">
            <label class="form-label" for="videoCategory"> Категория * </label>
            <div class="input-with-icon">
                <i class="fas fa-tags input-icon"></i>
                <select class="form-input" id="videoCategory" required="">
                    <option value="">Выберите категорию</option>
                    <option value="family">Семейное видео</option>
                    <option value="holiday">Праздники</option>
                    <option value="travel">Путешествия</option>
                    <option value="kids">Детское</option>
                    <option value="events">События</option>
                    <option value="other">Другое</option>
                </select>
            </div>
        </div> -->
    </form>
</div>

<style>
    /* Контент шагов - компактный */
    .step-content {
        padding: 25px 30px;
        display: none;
        animation: fadeIn 0.3s ease;
    }

    .step-content.active {
        display: block;
    }

    @keyframes fadeIn {
        from {
            opacity: 0;
            transform: translateY(5px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }

    .step-header {
        margin-bottom: 20px;
    }

    .step-title-main {
        font-size: 22px;
        font-weight: 700;
        margin-bottom: 5px;
        color: var(--text-primary);
    }

    .step-subtitle {
        color: var(--text-secondary);
        font-size: 13px;
    }

    /* Формы - компактные */
    .upload-form {
        width: 100%;
    }

    .form-group {
        margin-bottom: 18px;
    }

    .form-label {
        display: block;
        margin-bottom: 6px;
        font-weight: 600;
        color: var(--text-primary);
        font-size: 14px;
    }

    .form-label .optional {
        color: var(--text-muted);
        font-weight: normal;
        font-size: 12px;
        margin-left: 4px;
    }

    .input-with-icon {
        position: relative;
    }

    .input-icon {
        position: absolute;
        left: 12px;
        top: 50%;
        transform: translateY(-50%);
        color: var(--text-muted);
        font-size: 14px;
        z-index: 2;
    }

    .form-input {
        width: 100%;
        padding: 10px 12px 10px 36px;
        border: 1.5px solid var(--border-color);
        border-radius: 6px;
        font-size: 14px;
        font-weight: 500;
        color: var(--text-primary);
        background-color: rgba(30, 41, 59, 0.7);
        backdrop-filter: blur(10px);
        transition: var(--transition);
    }

    .form-input:focus {
        outline: none;
        border-color: var(--primary);
        box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.2);
        background-color: rgba(30, 41, 59, 0.9);
    }

    .form-input::placeholder {
        color: var(--text-muted);
        font-size: 13px;
    }

    /* Текстовое поле */
    .form-textarea {
        width: 100%;
        padding: 10px 12px;
        border: 1.5px solid var(--border-color);
        border-radius: 6px;
        font-size: 14px;
        font-weight: 500;
        color: var(--text-primary);
        background-color: rgba(30, 41, 59, 0.7);
        backdrop-filter: blur(10px);
        transition: var(--transition);
        min-height: 80px;
        resize: vertical;
        font-family: inherit;
    }

    .form-textarea:focus {
        outline: none;
        border-color: var(--primary);
        box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.2);
        background-color: rgba(30, 41, 59, 0.9);
    }

    /* Загрузка файла - компактная */
    .file-upload-area {
        border: 2px dashed var(--border-color);
        border-radius: 10px;
        padding: 30px 15px;
        text-align: center;
        cursor: pointer;
        transition: var(--transition);
        background: rgba(30, 41, 59, 0.3);
    }

    .file-upload-area:hover {
        border-color: var(--primary);
        background: rgba(59, 130, 246, 0.1);
    }

    .file-upload-area.drag-over {
        border-color: var(--success);
        background: rgba(16, 185, 129, 0.1);
    }

    .upload-icon {
        font-size: 36px;
        color: var(--primary);
        margin-bottom: 12px;
    }

    .upload-text {
        font-size: 15px;
        font-weight: 600;
        margin-bottom: 6px;
        color: var(--text-primary);
    }

    .upload-subtext {
        color: var(--text-muted);
        font-size: 12px;
        margin-bottom: 15px;
        max-width: 500px;
        margin-left: auto;
        margin-right: auto;
        line-height: 1.4;
    }

    .upload-button {
        display: inline-flex;
        align-items: center;
        gap: 8px;
        padding: 10px 18px;
        background: linear-gradient(
            135deg,
            var(--primary) 0%,
            var(--secondary) 100%
        );
        color: white;
        border: none;
        border-radius: 6px;
        font-size: 13px;
        font-weight: 600;
        cursor: pointer;
        transition: var(--transition);
    }

    .upload-button:hover {
        transform: translateY(-1px);
        box-shadow: 0 4px 12px rgba(59, 130, 246, 0.4);
    }

    .file-info {
        margin-top: 15px;
        padding: 15px;
        background: linear-gradient(
            135deg,
            rgba(16, 185, 129, 0.1) 0%,
            rgba(59, 130, 246, 0.1) 100%
        );
        border-radius: 8px;
        border-left: 3px solid var(--success);
        display: none;
    }

    .file-info.show {
        display: block;
        animation: fadeIn 0.5s ease;
    }

    .file-info-header {
        display: flex;
        align-items: center;
        gap: 8px;
        margin-bottom: 10px;
    }

    .file-info-icon {
        color: var(--success);
        font-size: 16px;
    }

    .file-name {
        font-weight: 600;
        font-size: 14px;
        color: var(--text-primary);
    }

    .file-details {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
        gap: 10px;
        margin-top: 10px;
    }

    .file-detail {
        display: flex;
        flex-direction: column;
        gap: 3px;
    }

    .detail-label {
        font-size: 11px;
        color: var(--text-muted);
        text-transform: uppercase;
        letter-spacing: 0.3px;
    }

    .detail-value {
        font-size: 12px;
        font-weight: 600;
        color: var(--text-primary);
    }
</style>
