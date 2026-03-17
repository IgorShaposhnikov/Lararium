export class Video {
    id = null;
    title = "no-video";
    //duration = "-1";
    //quality = "-1";
    //date = "";

    thumbnail = "https://images.unsplash.com/photo-1593359677879-a4bb92f829d1?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=500&q=80";
    summary = "";
    fileSize = "-1";
    actors = [];
    tags = [];
    fileType = "";
    fileExt = "";

    constructor(data) {
        Object.assign(this, data);
    }
}