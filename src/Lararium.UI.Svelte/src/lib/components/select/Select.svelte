<script lang="ts">
    import { Select } from "bits-ui";
    import {
        Check,
        Palette,
        ChevronsUpDown,
        ChevronUp,
        ChevronDown,
    } from "lucide-svelte";

    const themes = [
        { value: "light-monochrome", label: "Light Monochrome" },
        { value: "dark-green", label: "Dark Green" },
        { value: "svelte-orange", label: "Svelte Orange" },
        { value: "punk-pink", label: "Punk Pink" },
        { value: "ocean-blue", label: "Ocean Blue", disabled: true },
        { value: "sunset-orange", label: "Sunset Orange" },
        { value: "sunset-red", label: "Sunset Red" },
        { value: "forest-green", label: "Forest Green" },
        { value: "lavender-purple", label: "Lavender Purple", disabled: true },
        { value: "mustard-yellow", label: "Mustard Yellow" },
        { value: "slate-gray", label: "Slate Gray" },
        { value: "neon-green", label: "Neon Green" },
        { value: "coral-reef", label: "Coral Reef" },
        { value: "midnight-blue", label: "Midnight Blue" },
        { value: "crimson-red", label: "Crimson Red" },
        { value: "mint-green", label: "Mint Green" },
        { value: "pastel-pink", label: "Pastel Pink" },
        { value: "golden-yellow", label: "Golden Yellow" },
        { value: "deep-purple", label: "Deep Purple" },
        { value: "turquoise-blue", label: "Turquoise Blue" },
        { value: "burnt-orange", label: "Burnt Orange" },
    ];

    let value = $state("");
    const selectedLabel = $derived(
        themes.find((t) => t.value === value)?.label ?? "Select a theme",
    );
</script>

<Select.Root type="single" bind:value>
    <Select.Trigger
        class="
    inline-flex h-10 min-w-[240px] items-center rounded-md border border-nw-card bg-background px-3 text-sm transition-colors"
    >
        <Palette class="mr-2 size-4 opacity-50" />
        <span class="truncate">{selectedLabel}</span>
        <ChevronsUpDown class="ml-auto size-4 opacity-50" />
    </Select.Trigger>

    <Select.Portal>
        <Select.Content
            class="z-50 min-w-[var(--bits-select-anchor-width)] overflow-hidden rounded-md border bg-popover text-popover-foreground shadow-md data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0 data-[state=closed]:zoom-out-95 data-[state=open]:zoom-in-95"
            sideOffset={4}
        >
            <Select.ScrollUpButton
                class="flex cursor-default items-center justify-center py-1"
            >
                <ChevronUp class="size-4" />
            </Select.ScrollUpButton>

            <Select.Viewport class="p-1"></Select.Viewport>

            <Select.ScrollDownButton
                class="flex cursor-default items-center justify-center py-1"
            >
                <ChevronDown class="size-4" />
            </Select.ScrollDownButton>
        </Select.Content>
    </Select.Portal>
</Select.Root>

<style>
    :global([data-select-trigger]) {
        user-select: none;
        -webkit-tap-highlight-color: transparent;
    }

    :global([data-select-trigger]:focus) {
        outline: none;
    }

    :global([data-select-trigger]:focus-visible) {
        background-color: green;
        outline: auto;
            }
</style>
