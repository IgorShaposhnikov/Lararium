import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig, loadEnv } from 'vite';
import tailwindcss from '@tailwindcss/vite'
import basicSsl from '@vitejs/plugin-basic-ssl'

export default defineConfig(({ mode }) => {
    return {
        server: {
            https: true,
        },
        plugins: [basicSsl(), tailwindcss(), sveltekit()]
    }
});