<script>
    let { file = $bindable() } = $props();
    let fileInput;

    function formatBytes(bytes) {
        if (bytes === 0) return "0 Bytes";
        const k = 1024;
        const sizes = ["Bytes", "KB", "MB", "GB"];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + " " + sizes[i];
    }

    function handleFileChange(e) {
        const selectedFile = e.target.files[0];
        if (selectedFile) {
            file = selectedFile;
        }
    }

    let fileName = $derived(file ? file.name : "Файл не выбран");
    let fileSize = $derived(file ? formatBytes(file.size) : "0 MB");
    let fileType = $derived(
        file ? file.type.split("/")[1]?.toUpperCase() : "Видео",
    );
    let fileDate = $derived(
        file ? new Date(file.lastModified).toLocaleDateString() : "Сегодня",
    );
</script>

<div class="flex flex-col md:gap-8 gap-4 p-4" class:active={true} id="step2">
    <div class="flex flex-col gap-1">
        <h2 class="text-2xl font-bold text-l-foreground">Загрузка видео</h2>
        <p class="text-sm text-l-foreground-secondary">
            Загрузите ваш видеофайл
        </p>
    </div>

    <div class="flex flex-col md:gap-2 gap-1">
        <label
            for="video-uplading-dropzone"
            class="block mb-1.5 font-semibold text-sm text-l-foreground"
            >Видеофайл *</label
        >

        <div
            id="video-uplading-dropzone"
            role="button"
            tabindex="0"
            onkeydown={(e) => {
                if (e.key === "Enter" || e.key === " ") fileInput.click();
            }}
            ondragover={(e) => {
                e.preventDefault();
                isDragging = true;
            }}
            ondragleave={() => (isDragging = false)}
            ondrop={handleDrop}
            class="border-2 border-dashed border-l-border rounded-xl p-8 text-center cursor-pointer transition-all bg-l-control-bg hover:border-l-primary hover:bg-l-control-hover"
            onclick={() => fileInput.click()}
        >
            <div class="text-4xl text-l-primary mb-3">
                <i class="fas fa-cloud-upload-alt"></i>
            </div>
            <div class="text-base font-semibold mb-1.5 text-l-foreground">
                Перетащите видеофайл сюда
            </div>
            <div
                class="text-xs text-l-foreground-muted mb-4 max-w-[500px] mx-auto leading-relaxed"
            >
                Поддерживаемые форматы: MP4, AVI, MOV, MKV, WebM<br />
                Максимальный размер: 2 GB
            </div>

            <button
                type="button"
                class="inline-flex items-center gap-2 px-5 py-2.5 bg-gradient-to-r from-l-primary to-l-secondary text-white border-none rounded-md text-sm font-semibold cursor-pointer transition-all hover:-translate-y-0.5 hover:shadow-[0_4px_12px_rgba(59,130,246,0.4)]"
            >
                <i class="fas fa-folder-open"></i>
                Выбрать файл
            </button>

            <input
                bind:this={fileInput}
                onchange={handleFileChange}
                type="file"
                id="videoFile"
                accept="video/mp4,video/x-m4v,video/*"
                hidden
            />
        </div>

        {#if file}
            <div
                class="mt-4 p-4 bg-gradient-to-r from-l-success/10 to-l-primary/10 rounded-lg border-l-4 border-l-success animate-fadeIn"
            >
                <div class="flex items-center gap-2 mb-2.5">
                    <i class="fas fa-check-circle text-l-success text-base"></i>
                    <div class="font-semibold text-sm text-l-foreground">
                        {fileName}
                    </div>
                </div>
                <div
                    class="grid grid-cols-[repeat(auto-fit,minmax(150px,1fr))] gap-2.5 mt-2.5"
                >
                    <div class="flex flex-col gap-1">
                        <div
                            class="text-xs text-l-foreground-muted uppercase tracking-wide"
                        >
                            Размер
                        </div>
                        <div class="text-xs font-semibold text-l-foreground">
                            {fileSize}
                        </div>
                    </div>
                    <div class="flex flex-col gap-1">
                        <div
                            class="text-xs text-l-foreground-muted uppercase tracking-wide"
                        >
                            Тип
                        </div>
                        <div class="text-xs font-semibold text-l-foreground">
                            {fileType}
                        </div>
                    </div>
                    <div class="flex flex-col gap-1">
                        <div
                            class="text-xs text-l-foreground-muted uppercase tracking-wide"
                        >
                            Дата
                        </div>
                        <div class="text-xs font-semibold text-l-foreground">
                            {fileDate}
                        </div>
                    </div>
                </div>
            </div>
        {/if}
    </div>
</div>

<style>
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

    .animate-fadeIn {
        animation: fadeIn 0.3s ease;
    }
</style>
