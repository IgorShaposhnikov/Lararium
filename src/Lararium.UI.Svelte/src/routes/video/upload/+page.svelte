<!-- src/lib/components/Step1BasicInfo.svelte -->
<script>
    import Steps from "$lib/components/steps/Steps.svelte";
    import Step from "$lib/components/steps/Step.svelte";
    import GeneralInformation from "./_components/GeneralInformation.svelte";
    import VideoUploader from "./_components/VideoUploader.svelte";
    import { api } from "$lib/lararium/api";

    let uploadingData = $state({
        title: "",
        summary: "",
        formFile: null,
        actors: null,
        tags: null,
    });

    $inspect(uploadingData);

    let currentStep = $state(0);
    const totalSteps = 3;

    function next() {
        if (currentStep < totalSteps - 1) currentStep++;
    }

    function prev() {
        if (currentStep > 0) currentStep--;
    }

    async function uploadHlsVideo() {
        const endpoint = "/Video/upload/hls";

        // 1. Создаем FormData
        const formData = new FormData();

        // 2. Наполняем данными согласно вашему curl
        formData.append("Title", uploadingData.title);
        formData.append("Summary", uploadingData.summary);

        // Поле FormFile должно содержать объект File (из input) или Blob
        formData.append("FormFile", uploadingData.formFile);

        // Если Actors и MediaTags — это массивы, добавьте их в цикле,
        // либо по одному, если это одиночные значения
        if (uploadingData.actors) {
            formData.append("Actors", uploadingData.actors);
        }

        if (uploadingData.tags) {
            formData.append("MediaTags", uploadingData.tags);
        }

        try {
            // 3. Вызываем через ваш класс
            // Заголовок Content-Type удалится автоматически внутри метода request
            const response = await api.post(endpoint, formData);

            const result = await response.json();
            console.log("Видео успешно загружено:", result);
            return result;
        } catch (error) {
            console.error("Ошибка при загрузке:", error.message);
            throw error;
        }
    }
</script>

<div class="flex justify-center h-dvh p-4">
    <div class="flex flex-col w-5xl">
        <Steps bind:activeIndex={currentStep} class="flex-1 bg-red-500/0">
            <Step title="Начало">
                <GeneralInformation
                    bind:title={uploadingData.title}
                    bind:summary={uploadingData.summary}
                />
            </Step>
            <Step title="Процесс">
                <VideoUploader bind:file={uploadingData.formFile} />
            </Step>
            <Step title="Финиш">
                <h2 class="text-xl font-bold">Завершение</h2>
                <p>Вы великолепны!</p>
            </Step>
        </Steps>

        <div class="mt-6 flex justify-between items-center">
            <button
                class="px-6 py-2 border rounded-lg disabled:opacity-30"
                onclick={prev}
                disabled={currentStep === 0}
            >
                Назад
            </button>

            <span class="text-gray-500"
                >Шаг {currentStep + 1} из {totalSteps}</span
            >
            {#if totalSteps === currentStep + 1}
                <button
                    class="px-6 py-2 bg-purple-900 text-white rounded-lg disabled:opacity-30"
                    onclick={uploadHlsVideo}
                >
                    Загрузить
                </button>
            {:else}
                <button
                    class="px-6 py-2 bg-black text-white rounded-lg disabled:opacity-30"
                    onclick={next}
                    disabled={currentStep === totalSteps - 1}
                >
                    Далее
                </button>
            {/if}
        </div>
    </div>
</div>
