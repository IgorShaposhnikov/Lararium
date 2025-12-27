export class Video {
    id = 0;
    name = "no-video";
    duration = "-1";
    quality = "-1";
    size = "-1";
    tags = [];
    thumbnail = "null";
    date = "";
    constructor(data) {
        Object.assign(this, data);
    }
}