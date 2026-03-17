<script lang="js">
    import { setContext } from "svelte";
    import { STEPS_REGISTRY_CONTEXT_KEY } from "../constants";

    let {
        children,
        activeIndex = $bindable(0),
        class: className = "",
    } = $props();

    let stepKeys = $state([]);
    let stepsMetadata = $state({});

    function register(key, title) {
        if (!stepKeys.includes(key)) {
            stepKeys.push(key);
        }
        stepsMetadata[key] = title;

        return {
            get index() {
                return stepKeys.indexOf(key);
            },
            unregister: () => {
                stepKeys = stepKeys.filter((id) => id !== key);
                delete stepsMetadata[key];
                if (activeIndex >= stepKeys.length)
                    activeIndex = Math.max(0, stepKeys.length - 1);
            },
        };
    }

    setContext(STEPS_REGISTRY_CONTEXT_KEY, {
        register,
        get activeIndex() {
            return activeIndex;
        },
        set activeIndex(val) {
            activeIndex = val;
        },
    });
</script>

<div class="flex flex-col gap-8 w-full {className}">
    <div class="flex justify-between items-start relative px-4">
        {#each stepKeys as key, i}
            {@const isActive = activeIndex === i}
            <button
                class="flex flex-col items-center gap-3 z-10 group cursor-pointer"
                data-active={isActive ? "" : null}
                onclick={() => (activeIndex = i)}
            >
                <div
                    class="
                    w-10 h-10 rounded-full flex items-center justify-center border-2 transition-all duration-300
                    bg-l-background text-sm font-bold
                    data-[active]:bg-l-primary data-[active]:border-l-primary data-[active]:text-l-foreground
                    border-l-card-light text-l-foreground-muted group-hover:border-l-primary-light
                "
                >
                    {i + 1}
                </div>

                <span
                    class="
                    text-xs text-center max-w-[100px] transition-colors
                    data-[active]:text-l-foreground data-[active]:font-medium
                    text-l-foreground-muted
                "
                    data-active={isActive ? "" : null}
                >
                    {stepsMetadata[key] || `Step ${i + 1}`}
                </span>
            </button>
        {/each}

        <div
            class="absolute top-5 left-0 w-full h-0.5 bg-l-card-light -z-0"
        ></div>
    </div>

    <div class="w-full">
        {@render children?.()}
    </div>
</div>
