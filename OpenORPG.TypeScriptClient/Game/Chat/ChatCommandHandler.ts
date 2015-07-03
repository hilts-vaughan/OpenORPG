 module OpenORPG {
     
     export interface ICommandHandler {
         (message : Array<string>) : void;
     }

     /*
      * A class responsible for handling the various client sided commands 
      */
     export class ChatCommandHandler {
         
         private callbackTable = {};

         constructor() {
             
         }

         /*
          * Allows registration of callbacks from the outside world. The chat manager will
          * be responsible for matching and executing these. Callbacks will be fired accordingly.
          */
         registerCallback(command: CommandType, callback: ICommandHandler) {
             this.callbackTable[command] = callback;             
         }

         /*
          * Handles the incoming command type and parses them on the client sided of things.
          */
         handleCommand(command: CommandType, message: string) {            
             // Get the arguments to pass in here
             var args = this.getArgumentsFromMessage(message);
             this.callbackTable[command](args);
         }

         private getArgumentsFromMessage(message: string) : Array<string> {
             var args = message.match(/(?:[^\s"]+|"[^"]*")+/g);
             args.shift();
             return args;
         }


     }

 }