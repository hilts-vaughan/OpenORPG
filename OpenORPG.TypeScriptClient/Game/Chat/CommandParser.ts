module OpenORPG {
  
    /*
     * The command parser will take a given message and parse it for the expected result.
     * It will return the message type afterwards and then a 'value'. This value can be used
     * to determine the action to be performed.
     * 
     * Text that cannot be matched by the parser will be forwarded accordingly.
     */  
    export class CommandParser {
        
        private commandLookupTable = {};

        constructor() {
            this.generateMessageLookup();
        }


        /*
         * Generates a lookup table so that commands can have their types parsed correctly.
         */
        private generateMessageLookup() : void {
            this.addCommandToLookup("/echo", CommandType.Echo);
            this.addCommandToLookup("/fps", CommandType.Fps);            
        }

        /*
         * Adds a command by string to the lookup table
         */
        private addCommandToLookup(key: string, command: CommandType) {
            if (this.commandLookupTable[key])
                throw new Error("A command with the key " + key + " already exists for the command type " + command);
            this.commandLookupTable[key] = command;
        }

        /*
         * Parses a given message string and spits out an enumeration with the proper.
         */
        parseMessageType(message: string): CommandType {
            var key: string = message.split(' ')[0];

            if (this.commandLookupTable[key])
                return this.commandLookupTable[key];

            return CommandType.UnknownCommand;
        }


    }


} 