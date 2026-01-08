<script>
    import "./ProgressSteps.css";

    let { steps, currentStep = 1 } = $props();

    let CurrentStepContent = $state(null);
    let percentage = $state();

    $effect(() => {
        percentage = 100 / steps.length;
    });

    $effect(() => {
        updateSteps(currentStep);
        CurrentStepContent = steps[currentStep + 1].stepContent;
        console.log(CurrentStepContent);
    });

    function updateSteps(step) {
        steps.forEach((s, index) => {
            s.completed = s.number < step;
            s.active = s.number === step;
        });
    }

    function nextStepClicked() {
        if (steps.length + 1 != currentStep) {
            currentStep++;
        }
    }

    function prevStepClicked() {
        if (currentStep != 0) {
            currentStep--;
        }
    }
</script>

<div class="progress-steps">
    <div class="steps-container">
        <div class="steps-progress">
            <div
                class="progress-bar"
                style="width: {(currentStep - 1) * percentage}%;"
            ></div>
            {#each steps as step}
                <div
                    class="step {step.completed ? 'completed' : ''} {step.active
                        ? 'active'
                        : ''}"
                    data-step={step.number}
                >
                    <div class="step-circle">
                        {#if step.completed}
                            <i class="fas fa-check"></i>
                        {:else}
                            <span>{step.number}</span>
                        {/if}
                    </div>
                    <div class="step-title">{step.title}</div>
                </div>
            {/each}
        </div>

        {#if CurrentStepContent}
            <CurrentStepContent />
        {/if}

        <button onclick={prevStepClicked}>Back</button>
        <button onclick={nextStepClicked}>Next</button>
    </div>
</div>

<style src="./ProgressSteps.css"></style>
