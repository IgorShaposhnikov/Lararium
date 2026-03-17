<script lang="js">
    import { onMount, onDestroy, getContext } from "svelte";
    import { STEPS_REGISTRY_CONTEXT_KEY } from "../constants";

    let {
        children,
        title = "",
        key = crypto.randomUUID(),
        class: className = ""
    } = $props();

    const registry = getContext(STEPS_REGISTRY_CONTEXT_KEY);
    
    const self = registry.register(key, title);

    onDestroy(() => {
        self.unregister();
    });
</script>

{#if registry.activeIndex === self.index}
    <div class={className} data-active>
        {@render children?.()}
    </div>
{/if}
