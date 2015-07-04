///<reference path="Element.ts" />

module OpenORPG.UI {
    /**
     * DOCTODO
     */
    export class Checkbox extends Element {
        public get checked(): boolean {
            return typeof this.element.attr("checked") !== typeof undefined;
        }

        public set checked(value: boolean) {
            if (value) this.check();
            else this.uncheck();
        }

        public check(): void {
            this.element.attr("checked", "true");
        }

        public uncheck(): void {
            this.element.removeAttr("checked");
        }
    }
}