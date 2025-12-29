const API_URL = 'https://localhost:7098/api/v2';

import { auth } from '$lib/lararium/auth.svelte.js';
class LarariumApi {

    constructor(baseUrl, defaultHeaders = {}) {
        this.baseUrl = baseUrl;
        this.defaultHeaders = {
            'Content-Type': 'application/json',
            ...defaultHeaders
        };
    }

    async request(endpoint, data = null, headers = {}, options = {}) {

        if (auth.isAccessTokenExpired)
        {
            await auth.refresh();
        }

        if (auth.isRefreshTokenExpired)
        {
            auth.logout("rt401");
        }

        const url = `${this.baseUrl}${endpoint}`;

        const requestHeaders = {
            ...this.defaultHeaders,
            ...headers
        }

        requestHeaders['Authorization'] = `Bearer ${auth.accessToken}`;

        const config = {
            ...options,
            headers: requestHeaders
        };

        if (data !== null) {
            config.body = JSON.stringify(data);
        }

        const response = await fetch(url, config);

        if (response.status === 401) {
            auth.logout("ac401");
        }

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}: ${await response.text()}`);
        }

        return response;
    }

    async get(endpoint, data = null, headers = {}, options = {}) {
        return this.request(endpoint, data, headers, {
            method: 'GET',
            ...options
        });
    }

    async post(endpoint, data = null, headers = {}, options = {}) {
        return this.request(endpoint, headers, {
            method: 'POST',
            ...options
        });
    }

    async put(endpoint, data = null, headers = {}, options = {}) {
        return this.request(endpoint, {
            method: 'PUT',
            ...options
        });
    }

    async delete(endpoint, headers = {}, options = {}) {
        return this.request(endpoint, {
            method: 'DELETE',
            ...options
        });
    }
}

export const api = new LarariumApi(API_URL);