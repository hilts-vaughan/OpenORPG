GameClient = require "./Infrastructure/Network/GameClient.coffee"
MainMenuState = require "./Infrastructure/StateManagement/States/MainMenuState.coffee"
ErrorMenuState = require "./Infrastructure/StateManagement/States/ErrorMenuState.coffee"
BootstrapState = require "./Infrastructure/StateManagement/BootstrapState.coffee"

Debug = {}
Debug.AutoLogin = true

#  No parameters given, which means no default state is created or started
game = new Phaser.Game(1024, 768, Phaser.CANVAS, "gameContainer", null, true, true)
game.antialias = false

game.state.add "mainmenu", new MainMenuState(game)
game.state.add "errorstate", new ErrorMenuState(game)
game.state.add "preloader", new BootstrapState(game), true