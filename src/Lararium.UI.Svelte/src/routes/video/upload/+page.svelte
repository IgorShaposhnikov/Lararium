<script>
    import { api } from "$lib/lararium/api";
    import ProgressSteps from "./_components/ProgressSteps/ProgressSteps.svelte";
    import BasicInformationStep from "./_components/BasicInformationStep.svelte";
    import UploadVideoStep from "./_components/UploadVideoStep.svelte";

    let files = $state(null);

    import step1 from "./_components/step1.svelte";
    import step2 from "./_components/step2.svelte";

    let defaultSteps = $state([
        {
            number: 1,
            title: "Загрузка видео",
            completed: false,
            active: false,
            stepContent: step1,
        },
        {
            number: 2,
            title: "Основные данные",
            completed: false,
            active: false,
            stepContent: step2,
        },
        {
            number: 3,
            title: "Дополнительные данные",
            completed: false,
            active: false,
            stepContent: null,
        },
        {
            number: 4,
            title: "Завершение",
            completed: false,
            active: false,
            stepContent: null,
        },
    ]);

    let videoUploadData = $state({
        title: "",
        summary: "",
        formFile: null,
        actors: [],
        mediatags: [],
    });

    async function uploadVideo() {
        const formData = new FormData();
        formData.append("Title", videoUploadData.title);
        formData.append("Summary", videoUploadData.summary);
        formData.append("FormFile", videoUploadData.formFile); // Здесь сам файл

        try {
            let response = await api.post("/Video/upload/hls", formData);

            if (response.ok) {
                const result = await response.json();
                alert("Загрузка успешна!");
            } else {
                console.error("Ошибка сервера:", response.status);
            }
        } catch (error) {
            console.error("Ошибка сети:", error);
        }
    }
</script>

<div>
    <!-- <ProgressSteps steps={defaultSteps} /> -->

    <BasicInformationStep
        bind:title={videoUploadData.title}
        bind:summary={videoUploadData.summary}
    />
    <UploadVideoStep bind:file={videoUploadData.formFile} />

    <button
        class="step-button complete"
        id="completeUpload"
        onclick={uploadVideo}
    >
        <i class="fas fa-upload"></i>
        Загрузить видео
    </button>
</div>

<style>
    :root {
        --primary: #3b82f6;
        --primary-dark: #2563eb;
        --primary-light: #60a5fa;
        --secondary: #8b5cf6;
        --accent: #ec4899;
        --bg-dark: #0f172a;
        --bg-card: #1e293b;
        --bg-card-light: #334155;
        --text-primary: #f1f5f9;
        --text-secondary: #cbd5e1;
        --text-muted: #94a3b8;
        --border-color: #475569;
        --border-light: #64748b;
        --success: #10b981;
        --warning: #f59e0b;
        --danger: #ef4444;
        --border-radius: 12px;
        --shadow: 0 5px 20px rgba(0, 0, 0, 0.4);
        --shadow-hover: 0 8px 25px rgba(0, 0, 0, 0.5);
        --transition: all 0.3s ease;
    }
</style>
