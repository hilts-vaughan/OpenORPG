module OpenORPG {
    export class Interface {
        private static _modules: Dictionary<Module>;

        public static get modules(): Dictionary<Module> {
            if (this._modules === undefined || this._modules === null) {
                this._modules = new Dictionary<Module>();
            }

            return this._modules;
        }

        public static get game(): Module {
            return this.get('game');
        }

        public static get(name: string): Module {
            if (!this.modules.has(name)) {
                if (!this.modules.add(name, new Module(name))) {
                    throw new Error("Failed to add module '" + name + "'.");
                }
            }

            return this.modules.get(name);
        }

        public static del(name: string): boolean {
            return this.modules.del(name);
        }
    }
} 