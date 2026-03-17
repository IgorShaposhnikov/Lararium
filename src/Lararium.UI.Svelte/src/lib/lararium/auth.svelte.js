import { writable, derived } from 'svelte/store';
import { browser } from '$app/environment';
import { isTokenExpired, isRefreshTokenExpired } from '$lib/services/token-service';

const API_URL = '/api'; //import.meta.env.VITE_API_URL; //'

console.log(API_URL);

class LarariumAuthManager {
    accessToken = $state(null);
    refreshToken = $state(null);
    refreshTokenCreated = $state(null);
    refreshTokenExpires = $state(null);

    isAccessTokenExpired = $derived(isTokenExpired(this.accessToken));
    isRefreshTokenExpired = $derived(isRefreshTokenExpired(this.refreshTokenCreated, this.refreshTokenExpires));

    constructor(baseUrl, apiVersion = 'v1') {
        this.baseUrl = `${baseUrl}/${apiVersion}`;
        console.log(this.baseUrl);
    }

    async init() {
        if (browser) {
            await this.#preprocess_data();
        }
    }

    async login(credentials) {
        
        let url = `${this.baseUrl}/auth/login`;
        console.info(url);

        const headers = {
            'accept': 'text/plain; x-version=1.0',
            'Content-Type': 'application/json; x-version=1.0'
        };

        const response = await fetch(url, {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(credentials)
        })

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        var tokens = await response.json();

        this.#saveTokens(tokens);
    }

    async refresh() {
        let url = `${this.baseUrl}/auth/refresh`;

        const headers = {
            'accept': 'text/plain; x-version=1.0',
            'Content-Type': 'application/json; x-version=1.0'
        };

        console.info(url);

        const response = await fetch(url, {
            method: 'POST',
            headers: headers,
            body: JSON.stringify({
                "accessToken": this.accessToken,
                "refreshToken": this.refreshToken
            })
        })

        if (response.status == 401) {
            await this.logout("rt401");
        }

        if (!response.ok) {
            this.logout();
            const errorBody = await response.text();
            throw new Error(`HTTP error! status: ${response.status}\n - ${errorBody}`);
        }

        var tokens = await response.json();

        this.#saveTokens(tokens);
    }

    async logout(status) {
        localStorage.removeItem('access_token');
        localStorage.removeItem('refresh_token');
        localStorage.removeItem('refresh_token_expires');
        localStorage.removeItem('refresh_token_created');

        let loginPagePath = "/auth/sign-in";
        let isCurrentAuthPage = window.location.pathname.includes(loginPagePath);

        loginPagePath = `${loginPagePath}?status=${status}`

        if (status == 'rt401')
        {
            loginPagePath = `${loginPagePath}&returnUrl=${window.location.href}`
        }

        if (!isCurrentAuthPage) {
            window.location.href = loginPagePath;
        }
    }

    #saveTokens(tokens) {
        this.accessToken = tokens.accessToken;
        this.refreshToken = tokens.refreshToken.token;
        this.refreshTokenExpires = tokens.refreshToken.expires;
        this.refreshTokenCreated = tokens.refreshToken.created;

        localStorage.setItem('access_token', this.accessToken);
        localStorage.setItem('refresh_token', this.refreshToken);
        localStorage.setItem('refresh_token_created', this.refreshTokenCreated);
        localStorage.setItem('refresh_token_expires', this.refreshTokenExpires);
    }

    async #preprocess_data() {
        this.accessToken = localStorage.getItem('access_token');
        this.refreshToken = localStorage.getItem('refresh_token');
        this.refreshTokenCreated = localStorage.getItem('refresh_token_created');
        this.refreshTokenExpires = localStorage.getItem('refresh_token_expires');

        if (isRefreshTokenExpired(this.refreshTokenCreated, this.refreshTokenExpires)) {
            this.logout("rt401");
            return;
        }

        if (isTokenExpired(this.accessToken)) {
            await this.refresh();
        }
    }
}

// Export a single instance (Singleton pattern)
export const auth = new LarariumAuthManager(API_URL);
auth.init();
