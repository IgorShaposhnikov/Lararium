import { api } from "$lib/lararium/api";

const API_URL = '/api';// import.meta.env.VITE_API_URL;


class LarariumVideo {


async loadVideoStream(id) {
    console.log(API_URL);
    // так как blob не имеет свой домен, берем домен из window.location.origin, но для разработки нужно будет заменить на путь до сервера
    let serverSegmentUrl = `${window.location.origin}/api/${api.apiVersion}/video/${id}/segment/`;
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

export const larariumVideo = new LarariumVideo(API_URL);