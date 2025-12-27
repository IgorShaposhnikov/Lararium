

export const index = 2;
let component_cache;
export const component = async () => component_cache ??= (await import('../entries/pages/_page.svelte.js')).default;
export const imports = ["_app/immutable/nodes/2.BaBw0O8v.js","_app/immutable/chunks/C3r8np6y.js","_app/immutable/chunks/CRBACgz2.js","_app/immutable/chunks/DGmAC5S7.js"];
export const stylesheets = [];
export const fonts = [];
