module OpenORPG {
    export class StringIndexer<T> {
        [key: string]: T;
    }

    export class NumberIndexer<T> {
        [key: number]: T;
    }

    export interface IMapped<T> {
        indexer: NumberIndexer<T> | StringIndexer<T>;

        get(key: number | string): T;
        set(key: number | string, value: T): T;

        add(key: number | string, value: T): boolean;
        has(key: number | string): boolean;
        del(key: number | string): boolean;
    }

    export class Dictionary<T> implements IMapped<T> {
        private _indexer: StringIndexer<T>;

        constructor() {
            this._indexer = new StringIndexer<T>();
        }

        public get indexer(): StringIndexer<T> {
            return this._indexer;
        }

        public get(key: string): T {
            return this.indexer[key];
        }

        public set(key: string, value: T): T {
            var oldValue = this.indexer[key];

            this.indexer[key] = value;

            return oldValue;
        }

        public add(key: string, value: T): boolean {
            return this.set(key, value) !== value;
        }

        public has(key: string): boolean {
            return !(this.indexer[key] === undefined || this.indexer[key] === null);
        }

        public del(key: string): boolean {
            if (!this.has(key)) {
                return false;
            }

            this.indexer[key] = undefined;
            return true;
        }
    }

    export class Enumerated<T> implements IMapped<T> {
        private _indexer: NumberIndexer<T>;

        constructor() {
            this._indexer = new NumberIndexer<T>();
        }

        public get indexer(): NumberIndexer<T> {
            return this._indexer;
        }

        public get(key: number): T {
            return this.indexer[key];
        }

        public set(key: number, value: T): T {
            var oldValue = this.indexer[key];

            this.indexer[key] = value;

            return oldValue;
        }

        public add(key: number, value: T): boolean {
            return this.set(key, value) !== value;
        }

        public has(key: number): boolean {
            return !(this.indexer[key] === undefined || this.indexer[key] === null);
        }

        public del(key: number): boolean {
            if (!this.has(key)) {
                return false;
            }

            this.indexer[key] = undefined;
            return true;
        }
    }
}