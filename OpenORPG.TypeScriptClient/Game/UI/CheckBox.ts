///<reference path="Element.ts" />

module OpenORPG.UI {
    /**
     * DOCTODO
     */
    export class Checkbox extends Element {
        public get checked(): any {
            return Boolean(this.element.attr("ng-checked"));
        }

        public set checked(value: any) {
            if (typeof value == "number") {
                value = value == 0 ? false : true;
            }

            this.check(Boolean(value));
        }

        public check(value: boolean = true): void {
            if (value) {
                this.element.prop("checked", "checked");
            } else {
                this.element.removeProp("checked");
            }
        }

        public uncheck(): void {
            this.check(false);
        }
    }
}