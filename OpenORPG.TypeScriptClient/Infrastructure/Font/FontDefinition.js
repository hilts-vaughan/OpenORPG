var OpenORPG;
(function (OpenORPG) {
    var FontDefinition = (function () {
        function FontDefinition(font, fill, align, stroke, strokeThickness, wordWrap, wordWrapWidth) {
            this.font = font;
            this.fill = fill;
            this.align = align;
            this.stroke = stroke;
            this.strokeThickness = strokeThickness;
            this.wordWrap = wordWrap;
            this.wordWrapWidth = wordWrapWidth;
        }
        return FontDefinition;
    })();
    OpenORPG.FontDefinition = FontDefinition;
})(OpenORPG || (OpenORPG = {}));
//# sourceMappingURL=FontDefinition.js.map
