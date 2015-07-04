 module OpenORPG {
     
     export interface ICommandHandler {
         (message : Array<string>) : void;
     }

     /*
      * A class responsible for handling the various client sided commands 
      */
     export class ChatCommandHandler {
         
         private callbackTable = {};

         /*
          * Allows registration of callbacks from the outside world. The chat manager will
          * be responsible for matching and executing these. Callbacks will be fired accordingly.
          */
         registerCallback(command: CommandType, callback: ICommandHandler) {
             this.callbackTable[command] = callback;             
         }

         /*
          * Handles the incoming command type and attempts to fire the callback from the table.
            If it cannot be found, then it is ignored.
          */
         handleCommand(command: CommandType, message: string) {                         
             var args = this.getArgumentsFromMessage(message);
             if(this.callbackTable[command]) {
                this.callbackTable[command](args);
             }
         }
         
         /**
          * Given a string from the user input, returns the parameters from them.
          * Parameters are identified by spaces, unless they are double quoted. For example:
          * 
          * 'Vaughan Apple Banana' would be three parameters, ['Vaughan', 'Apple', 'Banana']
          * 'Vaughan "Apple Banana"' would be two parameters, ['Vaughan', 'Apple Banana']
          * 
          * The latter is primairly used for looking up user names and the like, things with
          * spaces in them. Generally, parameters are seperated by spaces.         
          */
         private getArgumentsFromMessage(message: string) : Array<string> {
             var args = message.match(/(?:[^\s"]+|"[^"]*")+/g);
             args.shift();
             return args;
         }
     }

 }