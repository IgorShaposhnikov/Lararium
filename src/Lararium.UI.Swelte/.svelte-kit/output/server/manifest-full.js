export const manifest = (() => {
function __memo(fn) {
	let value;
	return () => value ??= (value = fn());
}

return {
	appDir: "_app",
	appPath: "_app",
	assets: new Set(["images/0bf507d9-995b-0dbd-1efd-62aa384d90df","robots.txt"]),
	mimeTypes: {".txt":"text/plain"},
	_: {
		client: {start:"_app/immutable/entry/start.B6X3U8Dd.js",app:"_app/immutable/entry/app.B6XE08Dp.js",imports:["_app/immutable/entry/start.B6X3U8Dd.js","_app/immutable/chunks/CHiaenqD.js","_app/immutable/chunks/CRBACgz2.js","_app/immutable/chunks/Dju0TajG.js","_app/immutable/entry/app.B6XE08Dp.js","_app/immutable/chunks/CRBACgz2.js","_app/immutable/chunks/GwG4zLBi.js","_app/immutable/chunks/C3r8np6y.js","_app/immutable/chunks/Dju0TajG.js","_app/immutable/chunks/CKKHoOs8.js","_app/immutable/chunks/CCGQtAl8.js"],stylesheets:[],fonts:[],uses_env_dynamic_public:false},
		nodes: [
			__memo(() => import('./nodes/0.js')),
			__memo(() => import('./nodes/1.js')),
			__memo(() => import('./nodes/2.js')),
			__memo(() => import('./nodes/3.js'))
		],
		remotes: {
			
		},
		routes: [
			{
				id: "/",
				pattern: /^\/$/,
				params: [],
				page: { layouts: [0,], errors: [1,], leaf: 2 },
				endpoint: null
			},
			{
				id: "/video",
				pattern: /^\/video\/?$/,
				params: [],
				page: { layouts: [0,], errors: [1,], leaf: 3 },
				endpoint: null
			}
		],
		prerendered_routes: new Set([]),
		matchers: async () => {
			
			return {  };
		},
		server_assets: {}
	}
}
})();
