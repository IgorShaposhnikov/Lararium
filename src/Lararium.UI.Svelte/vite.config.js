import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig, loadEnv } from 'vite';
    import tailwindcss from '@tailwindcss/vite'
//import mkcert from 'vite-plugin-mkcert';

export default defineConfig(({ mode }) => {

    const env = loadEnv(mode, process.cwd(), '');

    return {
        server: {
            //https: true, // Включаем HTTPS для Vite dev server
            //port: 5173,
            //strictPort: true,
            //host: true, // Доступ по локальной сети
            
            proxy: {
                '/api': {
                    target: env.VITE_API_URL,
                    secure: false,
                    changeOrigin: true,
                    //ws: true,
                    rewrite: (path) => {
                        console.log('Proxying:', path, ' to ', env.VITE_API_URL)
                        return path.replace(/^\/api/, '');
                    }
                }
            },
        },
        plugins: [tailwindcss(), sveltekit()]//, mkcert()],
    }
});