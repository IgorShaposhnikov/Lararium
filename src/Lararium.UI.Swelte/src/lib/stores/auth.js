import { writable, derived } from 'svelte/store';
import { browser } from '$app/environment';

function createAuthStore() {
    const { subscribe, set, update } = writable({
        user: null,
        token: null,
        isAuthenticated: false,
        isLoading: true
    });
}

export const auth = createAuthStore();