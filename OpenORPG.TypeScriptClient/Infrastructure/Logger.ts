 module Logger {
  
     var log4javascript: any = window["log4javascript"].getLogger();
     var log = log4javascript;

     log.setLevel(window["log4javascript"]["Level"]["ALL"]);        
     var consoleLogger = new window["log4javascript"]["BrowserConsoleAppender"]();
     var popUpLayout = new window["log4javascript"]["PatternLayout"]("%d{HH:mm:ss} %-5p - %m%n");
     consoleLogger.setLayout(popUpLayout);
     consoleLogger.setThreshold(window["log4javascript"]["Level"]["TRACE"]);
     log.addAppender(consoleLogger);

     log.debug("Logging system has been booted up succesfully");

     export function trace(...params) {
         log.trace(params);
     }   

     export function debug(...params) {
         log.debug(params);
     }

     export function info(...params) {
         log.info(params);
     }

     export function warn(...params) {
         log.warn(params);
     }

     export function error(...params) {
         log.error(params);
     }

     export function fatal(...params) {
         log.fatal(params);
     }



 }