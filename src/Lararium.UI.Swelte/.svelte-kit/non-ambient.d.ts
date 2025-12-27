
// this file is generated — do not edit it


declare module "svelte/elements" {
	export interface HTMLAttributes<T> {
		'data-sveltekit-keepfocus'?: true | '' | 'off' | undefined | null;
		'data-sveltekit-noscroll'?: true | '' | 'off' | undefined | null;
		'data-sveltekit-preload-code'?:
			| true
			| ''
			| 'eager'
			| 'viewport'
			| 'hover'
			| 'tap'
			| 'off'
			| undefined
			| null;
		'data-sveltekit-preload-data'?: true | '' | 'hover' | 'tap' | 'off' | undefined | null;
		'data-sveltekit-reload'?: true | '' | 'off' | undefined | null;
		'data-sveltekit-replacestate'?: true | '' | 'off' | undefined | null;
	}
}

export {};


declare module "$app/types" {
	export interface AppTypes {
		RouteId(): "/" | "/auth" | "/auth/sign-in" | "/video" | "/video/_components";
		RouteParams(): {
			
		};
		LayoutParams(): {
			"/": Record<string, never>;
			"/auth": Record<string, never>;
			"/auth/sign-in": Record<string, never>;
			"/video": Record<string, never>;
			"/video/_components": Record<string, never>
		};
		Pathname(): "/" | "/auth" | "/auth/" | "/auth/sign-in" | "/auth/sign-in/" | "/video" | "/video/" | "/video/_components" | "/video/_components/";
		ResolvedPathname(): `${"" | `/${string}`}${ReturnType<AppTypes['Pathname']>}`;
		Asset(): "/images/0bf507d9-995b-0dbd-1efd-62aa384d90df" | "/robots.txt" | string & {};
	}
}