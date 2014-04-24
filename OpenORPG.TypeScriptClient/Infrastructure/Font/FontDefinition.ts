module OpenORPG {

    export class FontDefinition {
        public font: string;
        public fill: string;
        public align: string;
        public stroke: string;
        public strokeThickness: number;
        public wordWrap: boolean;
        public wordWrapWidth: number;

        constructor(font : string, fill : string, align : string, stroke?: string, strokeThickness? : number, wordWrap? : boolean, wordWrapWidth? : number) {
            this.font = font;
            this.fill = fill;
            this.align = align;
            this.stroke = stroke;
            this.strokeThickness = strokeThickness;
            this.wordWrap = wordWrap;
            this.wordWrapWidth = wordWrapWidth;            
        }

    }

}
