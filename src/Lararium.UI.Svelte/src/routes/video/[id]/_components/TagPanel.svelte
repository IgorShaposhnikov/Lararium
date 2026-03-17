<script>
    import Tag from "./Tag.svelte";

    let { tags = [] } = $props();
</script>

<div class="tags-section">
    <button class="add-tag-btn" id="addTagBtn" title="Добавить тег">
        <i class="fas fa-plus"></i>
    </button>
    <div class="tags-container" id="tagsContainer">
        {#each tags as tag}
            <Tag text={tag} />
        {/each}
    </div>
</div>

<style>
    /* Теги */
    .tags-section {
        display: flex;
        gap: 8px;
        margin-top: 10px;
    }

    .add-tag-btn {
        background-color: #2d3748;
        color: #e1e7ef;
        border: none;
        border-radius: 8px;
        width: 32px;
        height: 32px;
        cursor: pointer;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 14px;
        transition: background-color 0.2s;
        flex-shrink: 0;
    }

    .add-tag-btn:hover {
        background-color: #374151;
    }

    .tags-container {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
        align-items: center;
    }
</style>
