export function parseJwt(token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(
            atob(base64)
                .split('')
                .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                .join('')
        );
        return JSON.parse(jsonPayload);
    } catch {
        return null;
    }
}

export function isTokenExpired(token) {
    const payload = parseJwt(token);
    if (!payload || !payload.exp) return true;
    return payload.exp * 1000 < Date.now();
}

export function isRefreshTokenExpired(created, expired) {
    // 1. Проверяем что expired существует и это валидная дата
    if (!expired || isNaN(new Date(expired).getTime())) {
        console.warn('Invalid expiration date');
        return true;
    }

    // 2. Проверяем что created существует и это валидная дата
    if (!created || isNaN(new Date(created).getTime())) {
        console.warn('Invalid creation date');
        return true;
    }

    // 3. Проверяем что expired не в прошлом
    const now = Date.now();
    const expirationTime = new Date(expired).getTime();

    if (now >= expirationTime) {
        console.warn('Refresh token already expired');
        return true;
    }

    // 4. Проверяем что created не в будущем (с запасом 5 минут)
    const creationTime = new Date(created).getTime();
    const fiveMinutes = 5 * 60 * 1000;

    if (creationTime > (now + fiveMinutes)) {
        console.warn('Refresh token created in future');
        return true;
    }

    // 5. Дополнительно: проверяем что expired позже created
    if (expirationTime <= creationTime) {
        console.warn('Expiration time must be after creation time');
        return true;
    }

    return false;
}