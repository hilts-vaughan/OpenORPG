/**
 * DOCTODO
 *
 * @preferred
 */
module OpenORPG.UI {
    /**
     * DOCTODO
     */
    export class Element {
        /* ================================================================ *
         *                              Fields                              *
         * ================================================================ */

        /**
         * DOCTODO
         */
        private _parent: JQuery;

        /**
         * DOCTODO
         */
        private _element: JQuery;

        /**
         * DOCTODO
         */
        private _align: Align;

        /* ================================================================ *
         *                           Constructors                           *
         * ================================================================ */

        /**
         * DOCTODO
         *
         * @param parent
         * @param element
         */
        constructor(parent: JQuery, element: JQuery);

        /**
         * DOCTODO
         *
         * @param parent
         * @param elementIdOrSourceFile
         */
        constructor(parent: JQuery, elementIdOrSourceFile: string);

        /**
         * DOCTODO
         *
         * @param parent
         * @param element
         */
        constructor(parent: Element, element: JQuery);

        /**
         * DOCTODO
         *
         * @param parent
         * @param elementIdOrSourceFile
         */
        constructor(parent: Element, elementIdOrSourceFile: string);

        /**
         * DOCTODO
         */
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
                var selection = null;

                try {
                    selection = parent == null
                        ? $(element) /* If the parent is null, look top level. */
                        : parent.find(element); /* Or look at descendants. */
                } catch (e) {
                    /* Definitely not a selector */
                    selection = { length: 0 };
                }

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
                            $(that.parent).append(that.element);
                        } else {
                            /* If we don't have a parent, let's set it. */
                            that._parent = that.element.parent();
                        }
                        
                        /* VAUGHAN DOCTODO: Fill in this comment. */
                        angular.element(document).injector().invoke($compile => {
                            var container = that.element;
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

        /**
         * Used to initialize the element, retrieve values that it caches
         * and to bind events. Sub-classes that want to do initialization
         * work should have an override for the onLoad(JQueryEventObject)
         * method, which is called at the end of this method's execution.
         */
        private initialize(): void {
            this.refresh();

            /* Bind the most important events first */
            this.element.load(this.onLoad);
            this.element.unload(this.onUnload);

            /* Bind all of the other events */
            this.element.blur(this.onBlur);
            this.element.click(this.onClick);
            this.element.change(this.onChange);
            this.element.dblclick(this.onDoubleClick);
            this.element.focus(this.onFocus);
            this.element.keydown(this.onKeyDown);
            this.element.keypress(this.onKeyPress);
            this.element.keyup(this.onKeyUp);
            this.element.mousedown(this.onMouseDown);
            this.element.mousemove(this.onMouseMove);
            this.element.mouseout(this.onMouseOut);
            this.element.mouseover(this.onMouseOver);
            this.element.mouseup(this.onMouseUp);
            this.element.resize(this.onResize);
            this.element.scroll(this.onScroll);
            this.element.select(this.onSelect);
            this.element.submit(this.onSubmit);

            this.onLoad(null);
        }

        /* ================================================================ *
         *                           Cache methods                          *
         * ================================================================ */

        /**
         * Flushes properties that have not been updated to the DOM.
         */
        public flush(): void {
            this.flushAlign();
        }

        /**
         * Loads properties that need to be cached from the DOM and sets
         * the cache values accordingly.
         */
        public refresh(): void {
            this._align = getAlign(this.css("text-align"), this.css("vertical-align"));
        }

        /* ================================================================ *
         *                           Flush methods                          *
         * These should only be created and used for multi-value flushes.   *
         * ================================================================ */

        /**
         * Sets the Element's alignment CSS properties with the current value.
         */
        public flushAlign(): void {
            this.css("text-align", this.halign);
            this.css("vertical-align", this.valign);
        }

        /* ================================================================ *
         *                              Events                              *
         * Some of the events here are standard events, some are jQuery     *
         * specific. Note that any event that is explicitly marked as being *
         * non-standard should not be added. If it's present as jQuery but  *
         * not marked non-standard, it is fine to include it.               *
         * To determine whether it is explicitly non-standard, refer to:    *
         * https://developer.mozilla.org/en-US/docs/Web/API/HTMLElement     *
         * ================================================================ */

        /**
         * Called when the jQuery blur() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onBlur(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery change() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onChange(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery click() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onClick(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery dblclick() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onDoubleClick(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery focus() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onFocus(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery keydown() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onKeyDown(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery keypress() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onKeyPress(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery keyup() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onKeyUp(event: JQueryEventObject): void { }


        /**
         * Called when the jQuery load() event is invoked, or
         * in the Element.initialize() method, which is invoked
         * by the Element constructor.
         *
         * @param event an object containing the event information
         */
        protected onLoad(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery mosuedown() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onMouseDown(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery mousemove() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onMouseMove(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery mouseout() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onMouseOut(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery mouseover() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onMouseOver(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery mouseup() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onMouseUp(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery resize() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onResize(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery scroll() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onScroll(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery select() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onSelect(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery submit() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onSubmit(event: JQueryEventObject): void { }

        /**
         * Called when the jQuery unload() event is invoked.
         *
         * @param event an object containing the event information
         */
        protected onUnload(event: JQueryEventObject): void { }

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

        /**
         * Retrieves the string value of the specified CSS property.
         *
         * @param propertyName the name of the CSS property to retrieve
         * @returns the value of the CSS property
         */
        public css(propertyName: string): string;

        /**
         * Sets the values of multiple CSS properties.
         *
         * @param properties object of property-value pairs to set
         * @returns the JQuery object
         */
        public css(properties: Object): JQuery;

        /**
         * Sets the value of the specified CSS property.
         *
         * Refer to jQuery.css(propertyName, value) for further
         * reference as to what types of values are acceptable.
         * https://api.jquery.com/css/#css2
         *
         * @param propertyName the name of the CSS property to set
         * @param value the value to set the CSS property to
         * @returns the JQuery object
         */
        public css(propertyName: string, value: any): JQuery;

        /**
         * Used to interact with the jQuery.css() method group.
         */
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

        /**
         * Getts for the JQuery object for the parent element. This does
         * not return a parent Element object, as Element objects are
         * merely meant to serve as references to HTML elements, and
         * has no heiarchy of its own.
         *
         * @returns the JQuery object for the parent element
         */
        public get parent(): JQuery {
            return this._parent;
        }

        /**
         * Getter for the JQuery object for the element.
         *
         * @returns the JQuery object for the element
         */
        public get element(): JQuery {
            return this._element;
        }

        /**
         * Getter for the alignment of contents for this the element.
         *
         * @returns an Align enum
         */
        public get align(): Align {
            return this._align;
        }

        /**
         * Getter for the CSS horizontal alignment value, based on
         * the current value from the `align` getter. The return
         * values are valid values for the CSS `text-align` property.
         *
         * @returns the horizontal alignment CSS value
         */
        public get halign(): string {
            return getHAlignName(this._align);
        }

        /**
         * Getter for the CSS alignment value, based on
         * the current value from the `align` getter.
         * The return values are valid values for the
         * CSS `vertical-align` property.
         *
         * @returns the vertical alignment CSS value
         */
        public get valign(): string {
            return getVAlignName(this._align);
        }

        /**
         * Setter for this Elements content alignment. This affects
         * the positioning of the contents of this element, and does
         * not affect the position of the element itself.
         *
         * @param align the alignment value to set
         */
        public set align(align: Align) {
            this._align = align;
            this.flushAlign();
        }

        /**
         * DOCTODO
         *
         * @returns
         */
        public get backgroundColor(): string {
            return this.css("background-color");
        }

        /**
         * DOCTODO
         *
         * @param value
         */
        public set backgroundColor(value: string) {
            this.css("background-color", value);
        }

        /**
         * DOCTODO
         *
         * @returns
         */
        public get textColor(): string {
            return this.css("color");
        }

        /**
         * DOCTODO
         *
         * @param value
         */
        public set textColor(value: string) {
            this.css("color", value);
        }

        /**
         * DOCTODO
         *
         * @returns
         */
        public get textSize(): string {
            return this.css("font-size");
        }

        /**
         * DOCTODO
         *
         * @param value
         */
        public set textSize(value: string) {
            this.css("font-size", value);
        }

        /**
         * DOCTODO
         *
         * @returns
         */
        public get font(): string {
            return this.css("font-family");
        }

        /**
         * DOCTODO
         *
         * @param value
         */
        public set font(value: string) {
            this.css("font-family", value);
        }
    }
}