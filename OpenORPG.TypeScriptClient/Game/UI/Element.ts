/**
 * @preferred
 */
module OpenORPG.UI {
    export class Element {
        /* Fields */
        private _parent: JQuery;
        private _element: JQuery;
        private _align: Align;

        /* Constructors */
        constructor(parent: JQuery, element: JQuery);
        constructor(parent: JQuery, elementIdOrSourceFile: string);
        constructor(parent: Element, element: JQuery);
        constructor(parent: Element, elementIdOrSourceFile: string);
        constructor(parent: any, element: any) {
            if (parent instanceof Element) {
                /* If we were passed an actual UIElement,
                 * our registered parent is the JQuery
                 * element of our parent. */
                this._parent = (<Element>parent).element;
            } else {
                /* Otherwise this must just be our JQuery parent. */
                this._parent = parent;
            }

            if (typeof (element) !== "string") {
                /* If element is not a string it's a JQuery element */
                this._element = element;

                /* Let's initialize. */
                this.initialize();
            } else {
                /* Otherwise if it is a string it's a selector or source file */
                var selection =
                    parent == null
                        ? $(element) /* If the parent is null, look top level. */
                        : parent.find(element); /* Or look at descendants. */

                /* If it was a valid selector length will be non-zero. */
                if (selection.length > 0) {
                    this._element = selection.first();

                    /* Let's initialize. */
                    this.initialize();
                } else {
                    /* Assume that it's a source file */
                    var that = this;

                    /* We can instance our template path here
                     * to generate our DOM element */
                    $.get(element, data => {
                        /* Since we should be getting by id we only care
                         * about the first element, since there shouldn't
                         * be any others. */
                        that._element = $(data).first();

                        /* Before trying to just add the element to the parent,
                         * we need to make sure that there is a parent. */
                        if (that._parent != null) {
                            $(that._parent).append(that._element);
                        } else {
                            /* If we don't have a parent, let's set it. */
                            that._parent = that._element.parent();
                        }

                        angular.element(document).injector().invoke($compile => {
                            var container = that._element;
                            var scope = angular.element(container).scope();

                            $compile(container)(scope);
                            scope.$apply();
                        });

                        /* Since we finished loading from the source file,
                         * let's initialize. */
                        that.initialize();
                    });
                }
            }
        }

        private initialize(): void {
            this.refresh();

            this.onLoaded();
        }

        /* Flushes cached properties to the DOM */
        public flush(): void {
            this.flushAlign();
        }

        /* Loads cached properties from the DOM */
        public refresh(): void {
            this._align = getAlign(this.css("text-align"), this.css("vertical-align"));
        }

        /* Flush methods, these should only be written for multi-value flushes. */

        /**
         * Flushes the alignment
         */
        private flushAlign(): void {
            this.css("text-align", this.halign);
            this.css("vertical-align", this.valign);
        }

        /* Events */

        /**
         * Called when the object is initialized and ready.
         */
        protected onLoaded(): void {
            console.log(this.backgroundColor);
        }

        /* Functions */

        /**
         * Shows the Element.
         */
        public show(): void {
            this._element.show();
        }

        /**
         * Hides the Element.
         */
        public hide(): void {
            this._element.hide();
        }

        public css(propertyName: string): string;
        public css(properties: Object): JQuery;
        public css(propertyName: string, value: any): JQuery;
        public css(propertyNameOrProperties: any, value: any = null): any {
            /* If we have an Object of properties
             * let's return the JQuery object. */
            if (propertyNameOrProperties instanceof Object) {
                return this._element.css(propertyNameOrProperties);
            }

            /* Otherwise if we're just getting, return a string */
            if (value == null) {
                return this._element.css(propertyNameOrProperties);
            }

            /* Otherwise we are returning a JQuery object. */
            return this._element.css(propertyNameOrProperties, value);
        }



        public get parent(): JQuery {
            return this._parent;
        }

        public get element(): JQuery {
            return this._element;
        }

        public get align(): Align {
            return this._align;
        }

        public get halign(): string {
            return getHAlignName(this._align);
        }

        public get valign(): string {
            return getVAlignName(this._align);
        }

        public set align(align: Align) {
            this._align = align;
            this.flushAlign();
        }

        public get backgroundColor(): string {
            return this.css("background-color");
        }

        public set backgroundColor(value: string) {
            this.css("background-color", value);
        }

        public get textColor(): string {
            return this.css("color");
        }

        public set textColor(value: string) {
            this.css("color", value);
        }

        public get textSize(): string {
            return this.css("font-size");
        }

        public set textSize(value: string) {
            this.css("font-size", value);
        }

        public get font(): string {
            return this.css("font-family");
        }

        public set font(value: string) {
            this.css("font-family", value);
        }
    }
}