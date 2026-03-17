<!-- src/lib/components/Step1BasicInfo.svelte -->
<script>
    import Steps from "$lib/components/steps/Steps.svelte";
    import Step from "$lib/components/steps/Step.svelte";
    import GeneralInformation from "./_components/GeneralInformation.svelte";
    import VideoUploader from "./_components/VideoUploader.svelte";

    let currentStep = $state(0);
    const totalSteps = 3;

    function next() {
        if (currentStep < totalSteps - 1) currentStep++;
    }

    function prev() {
        if (currentStep > 0) currentStep--;
    }
</script>

<div class="flex justify-center h-dvh p-4">
    <div class="flex flex-col w-5xl">
        <Steps bind:activeIndex={currentStep} class="flex-1 bg-red-500/0">
            <Step title="Начало">
                <GeneralInformation />
            </Step>
            <Step title="Процесс">
                <VideoUploader />
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

            <button
                class="px-6 py-2 bg-black text-white rounded-lg disabled:opacity-30"
                onclick={next}
                disabled={currentStep === totalSteps - 1}
            >
                Далее
            </button>
        </div>
    </div>
</div>
