<script>
    import { api } from "$lib/lararium/api";
    import ProgressSteps from "./_components/ProgressSteps/ProgressSteps.svelte";

    let files = $state(null);

    $effect(async () => {
        if (!files || files.length === 0) return;

        const formData = new FormData();

        for (const file of files) {
            console.log(file);
            formData.append("file", file);
        }

        try {
            const response = await api.post("/Video/upload", formData);
            const result = await response.json();
            console.log("Успех:", result);
        } catch (error) {
            console.error("Ошибка загрузки:", error);
        }
    });

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
</script>

<div>
    <ProgressSteps steps={defaultSteps} />

    <!-- <label for="avatar">Upload a picture:</label>
    <!-- <input
        accept="video/png, image/jpeg"
        bind:files
        id="avatar"
        name="avatar"
        type="file"
    /> -->

    <!-- <input accept="video/*" bind:files id="avatar" name="avatar" type="file" /> -->
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
