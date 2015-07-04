module OpenORPG {
    /**
     * Alignment enum.
     * DOCTODO: This documentation is terrible (and I wrote it)
     */
    export enum Align {
        Center = 0xF, /* Left | Right or Top | Bottom */
        Left = 0xD, /* Top Left | Bottom Left */
        Right = 0xE, /* Top Right | Bottom Right */
        Top = 0x7, /* Top Left | Top Right */
        Bottom = 0xB, /* Bottom Left | Bottom Right */

        TopLeft = 0x5, /* Top & Left */
        TopRight = 0x6, /* Top & Right */

        BottomLeft = 0x9, /* Bottom & Left */
        BottomRight = 0xA, /* Bottom & Right */

        Default = TopLeft
    }

    /**
     * DOCTODO
     *
     * @param hname
     * @param vname
     * @returns
     */
    export function getAlign(hname: string, vname: string): Align {
        return getHAlign(hname) & getVAlign(vname);
    }

    /**
     * DOCTODO
     *
     * @param name
     * @returns
     */
    export function getHAlign(name: string): Align {
        switch (name) {
            case "center": return Align.Center;
            case "left": return Align.Left;
            case "right": return Align.Right;
            default: return Align.Left;
        }
    }

    /**
     * DOCTODO
     *
     * @param name
     * @returns
     */
    export function getVAlign(name: string): Align {
        switch (name) {
            case "baseline":
            case "middle":
                return Align.Center;

            case "text-top":
            case "super":
            case "top":
                return Align.Top;

            case "text-bottom":
            case "sub":
            case "bottom":
                return Align.Bottom;

            default: return Align.Top;
        }
    }

    /**
     * DOCTODO
     *
     * @param align
     * @returns
     */
    export function getHAlignName(align: Align): string {
        switch (align) {
            case Align.Left:
            case Align.TopLeft:
            case Align.BottomLeft:
                return "left";

            case Align.Center:
            case Align.Top:
            case Align.Bottom:
                return "center";

            case Align.Right:
            case Align.TopRight:
            case Align.BottomRight:
                return "right";

            default: return "top";
        }
    }

    /**
     * DOCTODO
     *
     * @param align
     * @returns
     */
    export function getVAlignName(align: Align): string {
        switch (align) {
            case Align.Center:
            case Align.Left:
            case Align.Right:
                return "middle";

            case Align.Top:
            case Align.TopLeft:
            case Align.TopRight:
                return "top";

            case Align.Bottom:
            case Align.BottomLeft:
            case Align.BottomRight:
                return "bottom";

            default: return "top";
        }
    }
} 