<script>
	import favicon from '$lib/assets/favicon.svg';

	let { children } = $props();
	export const ssr = false;
</script>

<svelte:head>
	<link rel="icon" href={favicon} />
	<link rel="stylesheet" href="/assets/fontawesome/7.0.0/css/all.min.css">
</svelte:head>

{@render children()}
