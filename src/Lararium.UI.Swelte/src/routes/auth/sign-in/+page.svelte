<script>
    import "./page.css";
    import { page } from "$app/state";
    import { goto } from "$app/navigation";
    import { auth } from "$lib/lararium/auth.svelte.js";
    import { onMount } from "svelte";
    import { enhance } from "$app/forms";

    let login = $state("string");
    let password = $state("string");

    onMount(() => {
        console.log(page.url.searchParams.get("returnUrl"));
    });

    let error = $state();

    async function loginClicked(e) {
        try {
            e.preventDefault();

            await auth.login({ login: "string", password: "string" });

            const returnUrl = page.url.searchParams.get("returnUrl") || "/dashboard";
            if (returnUrl.startsWith("http")) {
                window.location.href = returnUrl;
            } else {
                goto(returnUrl);
            }
        } catch (err) {
            error = err;
        }
    }

    export const ssr = false;
</script>

{#if error}
    <p>{error}</p>
{:else}
    <div class="login-container">
        <!-- Левая часть - компактная форма -->
        <div class="login-left">
            <div class="logo">
                <div class="logo-icon">
                    <i class="fas fa-home"></i>
                </div>
                <div class="logo-text">HomeHub</div>
            </div>

            <div class="welcome-section">
                <h1>Вход в домашнюю сеть</h1>
                <p>Только для авторизованных пользователей домашней сети</p>
            </div>

            <form class="login-form" id="loginForm" onsubmit={loginClicked}>
                <div class="form-group">
                    <label class="form-label" for="email"
                        >Имя пользователя</label
                    >
                    <div class="input-with-icon">
                        <i class="fas fa-user input-icon"></i>
                        <input
                            bind:value={login}
                            class="form-input"
                            type="text"
                            id="username"
                            placeholder="Введите имя пользователя"
                            required
                        />
                    </div>
                </div>

                <div class="form-group">
                    <label class="form-label" for="password">Пароль</label>
                    <div class="input-with-icon">
                        <i class="fas fa-lock input-icon"></i>
                        <input
                            bind:value={password}
                            class="form-input"
                            type="password"
                            id="password"
                            placeholder="Введите пароль"
                            required
                        />
                        <button
                            type="button"
                            class="password-toggle"
                            id="togglePassword"
                        >
                            <i class="fas fa-eye"></i>
                        </button>
                    </div>
                </div>

                <div class="form-options">
                    <div class="remember-me">
                        <label class="checkbox-container">
                            <input type="checkbox" id="remember" />
                            <span class="checkmark"></span>
                        </label>
                        <span class="remember-text"
                            >Запомнить на этом устройстве</span
                        >
                    </div>
                    <a href="#" class="forgot-password" id="forgotPassword">
                        Забыли пароль?
                    </a>
                </div>

                <button class="submit-button">
                    <i class="fas fa-sign-in-alt"></i>
                    Войти в домашнюю сеть
                </button>

                <div class="local-network-info">
                    <div class="network-info-title">
                        <i class="fas fa-network-wired"></i>
                        Информация о домашней сети
                    </div>
                    <div class="network-info-text">
                        Доступ разрешен только для устройств в вашей локальной
                        сети. Для настройки пользователей обратитесь к
                        администратору домашней сети.
                    </div>
                </div>
            </form>
        </div>

        <!-- Правая часть - функции приложения -->
        <div class="login-right">
            <div class="right-content">
                <h2 class="features-title">Домашняя платформа HomeHub</h2>
                <p class="features-subtitle">
                    Все необходимое для управления домашней жизнью
                </p>

                <div class="features-grid">
                    <div class="feature-card">
                        <div class="feature-icon">
                            <i class="fas fa-comments"></i>
                        </div>
                        <h3 class="feature-title">Локальный чат</h3>
                        <p class="feature-description">
                            Общайтесь в домашней сети
                        </p>
                    </div>

                    <div class="feature-card">
                        <div class="feature-icon">
                            <i class="fas fa-folder"></i>
                        </div>
                        <h3 class="feature-title">Файловое хранилище</h3>
                        <p class="feature-description">
                            Общие документы и медиафайлы
                        </p>
                    </div>

                    <div class="feature-card">
                        <div class="feature-icon">
                            <i class="fas fa-chart-pie"></i>
                        </div>
                        <h3 class="feature-title">Семейный бюджет</h3>
                        <p class="feature-description">
                            Учет расходов и финансов
                        </p>
                    </div>

                    <div class="feature-card">
                        <div class="feature-icon">
                            <i class="fas fa-calendar"></i>
                        </div>
                        <h3 class="feature-title">Семейный календарь</h3>
                        <p class="feature-description">
                            Планирование семейных дел
                        </p>
                    </div>

                    <div class="feature-card">
                        <div class="feature-icon">
                            <i class="fas fa-tasks"></i>
                        </div>
                        <h3 class="feature-title">Домашние задачи</h3>
                        <p class="feature-description">
                            Распределение обязанностей
                        </p>
                    </div>

                    <div class="feature-card">
                        <div class="feature-icon">
                            <i class="fas fa-shield-alt"></i>
                        </div>
                        <h3 class="feature-title">Безопасность</h3>
                        <p class="feature-description">
                            Контроль доступа в сети
                        </p>
                    </div>
                </div>

                <div class="app-stats">
                    <div class="stat-item">
                        <div class="stat-number">Локальная</div>
                        <div class="stat-label">Сеть</div>
                    </div>
                    <div class="stat-item">
                        <div class="stat-number">Защищенный</div>
                        <div class="stat-label">Доступ</div>
                    </div>
                    <div class="stat-item">
                        <div class="stat-number">Семейный</div>
                        <div class="stat-label">Формат</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
{/if}

<style src="./page.css"></style>
