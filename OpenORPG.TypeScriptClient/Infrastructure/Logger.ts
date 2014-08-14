 module Logger {
  
     var log4javascript: any = window["log4javascript"].getLogger();
     var log = log4javascript;

     log.setLevel(window["log4javascript"]["Level"]["ALL"]);        
     var consoleLogger = new window["log4javascript"]["BrowserConsoleAppender"]();
     var popUpLayout = new window["log4javascript"]["PatternLayout"]("%d{HH:mm:ss} %-5p - %m");
     consoleLogger.setLayout(popUpLayout);
     consoleLogger.setThreshold(window["log4javascript"]["Level"]["TRACE"]);
     log.addAppender(consoleLogger);

     log.debug("Logging system has been booted up succesfully");

     export function trace(params : any) {
         log.trace(params);
     }   

     export function debug(params : any) {
         log.debug(<Object> params);
     }

     export function info(x : any) {
         log.info(x);
     }

     export function warn(x : any) {
         log.warn(x);
     }

     export function error(x : any) {
         log.error(x);
     }

     export function fatal(x : any) {
         log.fatal(x);
     }



 }