import { PUBLIC_API_URL } from '$env/static/public';

console.log(PUBLIC_API_URL);

import { auth } from '$lib/lararium/auth.svelte.js';
class LarariumApi {

    constructor(baseUrl, apiVersion, defaultHeaders = {}) {
        this.baseUrl = baseUrl;
        this.apiVersion = apiVersion;
        this.defaultHeaders = {
            'Content-Type': 'application/json',
            ...defaultHeaders
        };

        console.log(baseUrl);
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

        const url = `${this.baseUrl}/${this.apiVersion}${endpoint}`;

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
            if (data instanceof FormData) {
                delete config.headers['Content-Type'];
                config.body = data;
            } else {
                config.body = JSON.stringify(data);
            }
        }

        console.info(config);

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
        return this.request(endpoint, data, headers, {
            method: 'POST',
            ...options
        });
    }

    async put(endpoint, data = null, headers = {}, options = {}) {
        return this.request(endpoint, data, {
            method: 'PUT',
            ...options
        });
    }

    async delete(endpoint, headers = {}, options = {}) {
        return this.request(endpoint, data, {
            method: 'DELETE',
            ...options
        });
    }
}

export const api = new LarariumApi(PUBLIC_API_URL, 'v1');
export const apiV2 = new LarariumApi(PUBLIC_API_URL, 'v2');