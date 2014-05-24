module Models {
    
    export class Item {
        name: string;
        description: string;
        price : number;
        type: ItemType;
    }


    export enum ItemType {
        FieldItem,
        Equipment
    }

} 