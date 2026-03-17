import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig, loadEnv } from 'vite';
import tailwindcss from '@tailwindcss/vite'

export default defineConfig(({ mode }) => {
    return {
        plugins: [tailwindcss(), sveltekit()]//, mkcert()],
    }
});