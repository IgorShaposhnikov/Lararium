<script>
	import favicon from '$lib/assets/favicon.svg';
	// import font_awesome from '$lib/assets/css/font-awesome/6.4.0/all.min.css'

	let { children } = $props();
</script>

<svelte:head>
	<link rel="icon" href={favicon} />
	<!-- <link rel="stylesheet" href="{font_awesome}"> -->
</svelte:head>

{@render children()}
