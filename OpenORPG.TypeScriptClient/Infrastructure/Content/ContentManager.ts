module OpenORPG {
    
    /*
     * The ContentManager is responsible for loading in various game files and keeping them cached.
     */
    export class ContentManager {

        // an internal private cache
        private contentCache : Object;
        private static _instance: ContentManager = null;

        constructor() {
            this.contentCache = {};
        }

        getContent(type: ContentType, id: number, callback: Function) {

            var name = ContentType[type].toLowerCase() + "s";
            var cache = this.contentCache[name];

            if (!cache)
                this.contentCache[name] = {};

            // Attempt to fetch the content from the cache
            var content = this.contentCache[name][id];
            var that = this;

            if (content)
                callback(content);
            else {
                $.getJSON(DirectoryHelper.getGameFilesPath() + name + "/" + id.toString() + ".json", (data) => {
                    that.contentCache[name][id] = data;
                    callback(data);
                });
            }

        }

        public static getInstance(): ContentManager {
            if (ContentManager._instance === null) {
                ContentManager._instance = new ContentManager();
            }
            return ContentManager._instance;
        }


    }

}