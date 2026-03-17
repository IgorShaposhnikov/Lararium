import { api } from "$lib/lararium/api";
import { PUBLIC_API_URL } from '$env/static/public';

class LarariumVideo {
    async loadVideoStream(id) {
        console.log(PUBLIC_API_URL);
        // так как blob не имеет свой домен, указываем путь до публичного API
        let serverSegmentUrl = `${PUBLIC_API_URL}/${api.apiVersion}/video/${id}/segment/`;
        let videoUrlApi = `/video/${id}/stream`;

        const response = await api.get(videoUrlApi);

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        let contentType = response.headers.get("content-type");
        let contentSize = response.headers.get("content-length");

        console.warn(contentType);

        if (contentType === "application/vnd.apple.mpegurl; x-version=1") {

            var m3u8Content = await response.text();
            m3u8Content = m3u8Content.replaceAll("{baseUrl}", serverSegmentUrl);
            console.log(serverSegmentUrl);
            console.log(m3u8Content);

            const blob = new Blob([m3u8Content], { type: 'application/x-mpegURL' });
            const blobUrl = URL.createObjectURL(blob);
            return blobUrl;
        }

        const blob = await response.blob();
        return URL.createObjectURL(blob);
    };
}

export const larariumVideo = new LarariumVideo();