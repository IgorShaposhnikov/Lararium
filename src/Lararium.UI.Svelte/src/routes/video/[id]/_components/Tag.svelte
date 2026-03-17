<script>
    let { text = "tag" } = $props();
</script>

<div class="tag">
    {text}
</div>

<style>
    .tag {
        background-color: #2d3748;
        color: #e1e7ef;
        padding: 2px 8px;
        border-radius: 12px;
        font-size: 12.5px;
    }
</style>
