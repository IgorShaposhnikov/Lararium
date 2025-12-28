Да, можно сделать почти как атрибут `[Authorize]` в Blazor! Вот несколько подходов:

## **1. Вариант с Guard компонентом (самый простой)**

```svelte
<!-- src/lib/components/Guard.svelte -->
<script>
  import { auth } from '$lib/stores/auth';
  import { goto } from '$app/navigation';
  import { onMount } from 'svelte';

  export let roles = [];
  export let redirectTo = '/login';
  export let fallback = null; // Компонент для показа при отсутствии доступа

  let hasAccess = false;
  let isLoading = true;

  onMount(() => {
    const unsubscribe = auth.subscribe(($auth) => {
      isLoading = $auth.isLoading;

      if (!$auth.isLoading) {
        // Проверяем авторизацию
        if (!$auth.isAuthenticated) {
          goto(redirectTo);
          return;
        }

        // Проверяем роли
        if (roles.length > 0 && $auth.user) {
          const hasRole = roles.some(role => 
            $auth.user.roles.includes(role)
          );
          if (!hasRole) {
            if (fallback) {
              hasAccess = false;
            } else {
              goto('/unauthorized');
            }
            return;
          }
        }

        hasAccess = true;
      }
    });

    return unsubscribe;
  });
</script>

{#if isLoading}
  <div class="auth-loading">Проверка доступа...</div>
{:else if hasAccess}
  <slot />
{:else if fallback}
  <svelte:component this={fallback} />
{/if}

<style>
  .auth-loading {
    text-align: center;
    padding: 2rem;
  }
</style>
```

**Использование как "атрибута":**
```svelte
<!-- src/routes/dashboard/+page.svelte -->
<script>
  import Guard from '$lib/components/Guard.svelte';
  import Unauthorized from '$lib/components/Unauthorized.svelte';
</script>

<Guard roles={['Admin']} fallback={Unauthorized}>
  <h1>Админ панель</h1>
  <!-- защищенный контент -->
</Guard>
```

## **2. Вариант с Higher-Order Component (HOC)**

```javascript
// src/lib/hocs/withAuth.js
import { auth } from '$lib/stores/auth';
import { goto } from '$app/navigation';
import { onMount } from 'svelte';

export function withAuth(Component, options = {}) {
  return function WrappedComponent(props) {
    let Wrapper;

    onMount(() => {
      const unsubscribe = auth.subscribe(($auth) => {
        if (!$auth.isLoading) {
          // Проверка авторизации
          if (!$auth.isAuthenticated) {
            goto(options.redirectTo || '/login');
            return;
          }

          // Проверка ролей
          if (options.roles && options.roles.length > 0 && $auth.user) {
            const hasRole = options.roles.some(role => 
              $auth.user.roles.includes(role)
            );
            if (!hasRole) {
              goto(options.unauthorizedRedirect || '/unauthorized');
              return;
            }
          }
        }
      });

      return unsubscribe;
    });

    // Рендерим оригинальный компонент
    Wrapper = Component;

    return Wrapper;
  };
}
```

**Использование:**
```javascript
// src/routes/dashboard/+page.svelte
<script>
  import { withAuth } from '$lib/hocs/withAuth';

  // Декорируем компонент
  export let component = withAuth(DashboardPage, {
    roles: ['Admin'],
    redirectTo: '/login'
  });
</script>

<svelte:component this={component} />
```

## **3. Вариант с метаданными маршрутов (самый близкий к атрибутам)**

```javascript
// src/lib/auth/routes.js
export const routeGuards = {
  '/dashboard': { 
    requireAuth: true, 
    roles: ['User'] 
  },
  '/admin': { 
    requireAuth: true, 
    roles: ['Admin'] 
  },
  '/profile': { 
    requireAuth: true 
  },
  '/login': { 
    requireAuth: false, // Публичный
    redirectIfAuth: '/' // Если авторизован - редирект
  },
  '/register': { 
    requireAuth: false 
  }
};

// Функция проверки доступа для текущего пути
export function checkRouteAccess(path, auth) {
  const guard = routeGuards[path] || {};

  // Если маршрут не найден - разрешаем доступ (или запрещаем)
  if (!guard) return { allowed: true };

  // Проверка: если авторизован, но маршрут требует отсутствия авторизации
  if (guard.redirectIfAuth && auth.isAuthenticated) {
    return { 
      allowed: false, 
      redirect: guard.redirectIfAuth 
    };
  }

  // Проверка авторизации
  if (guard.requireAuth && !auth.isAuthenticated) {
    return { 
      allowed: false, 
      redirect: '/login' 
    };
  }

  // Проверка ролей
  if (guard.roles && guard.roles.length > 0) {
    const hasRole = guard.roles.some(role => 
      auth.user?.roles.includes(role)
    );
    if (!hasRole) {
      return { 
        allowed: false, 
        redirect: '/unauthorized' 
      };
    }
  }

  return { allowed: true };
}
```

**Глобальный layout с автопроверкой:**
```svelte
<!-- src/routes/+layout.svelte -->
<script>
  import { auth } from '$lib/stores/auth';
  import { page } from '$app/stores';
  import { goto } from '$app/navigation';
  import { checkRouteAccess } from '$lib/auth/routes';

  $: {
    if (!auth.isLoading) {
      const access = checkRouteAccess($page.url.pathname, $auth);
      if (!access.allowed) {
        goto(access.redirect);
      }
    }
  }
</script>

<slot />
```

## **4. Вариант с пользовательскими layout (самый элегантный)**

```svelte
<!-- src/routes/(auth)/+layout.svelte -->
<script>
  import { auth } from '$lib/stores/auth';
  import { goto } from '$app/navigation';
  import { onMount } from 'svelte';

  export let roles = [];

  onMount(() => {
    const unsubscribe = auth.subscribe(($auth) => {
      if (!$auth.isLoading) {
        // Проверка авторизации
        if (!$auth.isAuthenticated) {
          goto('/login');
          return;
        }

        // Проверка ролей
        if (roles.length > 0 && $auth.user) {
          const hasRole = roles.some(role => 
            $auth.user.roles.includes(role)
          );
          if (!hasRole) {
            goto('/unauthorized');
            return;
          }
        }
      }
    });

    return unsubscribe;
  });
</script>

<slot />
```

**Структура проекта:**
```
src/routes/
├── +layout.svelte                 # Главный layout (публичный)
├── login/
│   └── +page.svelte              # Страница логина
├── register/
│   └── +page.svelte              # Регистрация
├── (auth)/                       # Группа защищенных маршрутов
│   ├── +layout.svelte            # Защищенный layout
│   ├── dashboard/
│   │   └── +page.svelte          # Автоматически защищена!
│   └── profile/
│       └── +page.svelte          # Тоже защищена!
└── (admin)/                      # Группа для админов
    ├── +layout.svelte            # Layout с проверкой роли
    │   <!-- script с roles={['Admin']} -->
    └── admin-panel/
        └── +page.svelte          # Только для админов
```

**Использование:**
```svelte
<!-- src/routes/(auth)/+layout.svelte -->
<!-- Автоматически защищает все вложенные маршруты -->
<!-- Ничего не нужно писать в дочерних страницах! -->

<!-- src/routes/(admin)/+layout.svelte -->
<script>
  // Только для админов
  export let roles = ['Admin'];
</script>

<slot />
```

## **5. Миксины/Хуки (аналог middleware)**

```javascript
// src/hooks/auth.js
import { auth } from '$lib/stores/auth';
import { goto } from '$app/navigation';

export function useAuthGuard(options = {}) {
  return {
    mounted() {
      const unsubscribe = auth.subscribe(($auth) => {
        if (!$auth.isLoading) {
          // Проверка авторизации
          if (!$auth.isAuthenticated) {
            goto(options.redirectTo || '/login');
            return;
          }

          // Проверка ролей
          if (options.roles && options.roles.length > 0 && $auth.user) {
            const hasRole = options.roles.some(role => 
              $auth.user.roles.includes(role)
            );
            if (!hasRole) {
              goto(options.unauthorizedRedirect || '/unauthorized');
              return;
            }
          }
        }
      });

      return unsubscribe;
    }
  };
}
```

**Использование:**
```svelte
<script>
  import { useAuthGuard } from '$hooks/auth';
  import { onMount } from 'svelte';

  // Используем хук
  const authGuard = useAuthGuard({ roles: ['Admin'] });

  onMount(() => {
    authGuard.mounted();
  });
</script>

<!-- Контент доступен только после проверки -->
<h1>Защищенная страница</h1>
```

## **6. Рекомендуемый подход: Группы маршрутов + Guard компонент**

**Для большинства случаев лучше всего использовать комбинацию:**

1. **Публичные маршруты** - без защиты
2. **(auth) группа** - для всех авторизованных
3. **(role) группы** - для конкретных ролей
4. **Guard компонент** - для дополнительных проверок внутри страниц

**Пример итоговой структуры:**
```svelte
<!-- Страница с проверкой конкретного условия -->
<script>
  import Guard from '$lib/components/Guard.svelte';
</script>

<Guard roles={['Premium']}>
  <h1>Премиум контент</h1>
  <!-- Только для Premium пользователей -->
</Guard>
```

**Преимущества этого подхода:**
- ✅ Максимально похоже на `[Authorize(Roles = "Admin")]`
- ✅ Минимальный код в компонентах
- ✅ Гибкая настройка на уровне групп
- ✅ Возможность точечной защиты
- ✅ Легко поддерживать

**Именно так и делают в production-проектах на SvelteKit!**
