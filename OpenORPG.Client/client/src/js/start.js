require('./Infrastructure/Network/GameClient.js');
require('./Infrastructure/StateManagement/States/BaseMenuState.js');
require('./Infrastructure/StateManagement/States/MainMenuState.js');
require('./Infrastructure/StateManagement/States/ErrorMenuState.js');
require('./Infrastructure/StateManagement/BootstrapState.js');


Debug = {};

Debug.AutoLogin = true;


//  No parameters given, which means no default state is created or started
var game = new Phaser.Game(1024, 768, Phaser.CANVAS, 'gameContainer', null, true, true);
game.antialias = false;

game.state.add('mainmenu', new MainMenuState(game));
game.state.add('errorstate', new ErrorMenuState(game));
game.state.add('preloader', new BootstrapState(game), true)