<script>
    let { title = $bindable(), summary = $bindable() } = $props();
</script>

<div class="flex flex-col gap-4 p-4">
    <div class="mb-5 flex flex-col gap-1">
        <h2 class="text-2xl font-bold mb-1 text-l-text-primary">
            Основная информация
        </h2>
        <p class="text-sm text-l-text-secondary">
            Введите основную информацию о вашем видео
        </p>
    </div>

    <form class="w-full">
    <!-- Группа Название -->
    <div class="mb-4">
        <label class="block mb-1.5 text-sm font-semibold text-l-text-primary" for="videoTitle">
            Название видео *
        </label>
        <div class="relative">
            <i class="fas fa-heading absolute left-3 top-1/2 -translate-y-1/2 text-sm text-l-muted z-10"></i>
            <input
                class="w-full py-2.5 pr-3 pl-9 bg-l-card/70 backdrop-blur-md 
                       border border-l-border rounded-md text-sm font-medium 
                       text-l-text-primary transition-all outline-hidden
                       focus:border-l-primary focus:ring-2 focus:ring-l-primary/20 
                       focus:bg-l-card/90 placeholder:text-l-muted placeholder:text-xs"
                type="text"
                id="videoTitle"
                placeholder="Введите название видео"
                required
                bind:value={title}
            />
        </div>
    </div>

    <!-- Группа Описание -->
    <div class="mb-4">
        <label class="block mb-1.5 text-sm font-semibold text-l-text-primary" for="videoDescription">
            Описание <span class="ml-1 text-xs font-normal text-l-muted">(необязательно)</span>
        </label>
        <textarea
            class="w-full p-3 bg-l-card/70 backdrop-blur-md 
                   border border-l-border rounded-md text-sm font-medium 
                   text-l-text-primary transition-all outline-hidden min-h-[80px] resize-y
                   focus:border-l-primary focus:ring-2 focus:ring-l-primary/20 
                   focus:bg-l-card/90 placeholder:text-l-muted"
            id="videoDescription"
            placeholder="Опишите ваше видео..."
            bind:value={summary}
        ></textarea>
    </div>
</form>
</div>
