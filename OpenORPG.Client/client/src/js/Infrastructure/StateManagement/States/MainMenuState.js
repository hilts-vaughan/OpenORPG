/*
        This is a state that shows the main menu of the game, it allows the player to work within a UI to reach the gameplay screen.
*/

require('./../../PacketTypes.js');
require('./../../../Game/StateManagement/GameplayState.js')
require('./../../../Game/StateManagement/HeroSelectState.js')

module.exports = function() {

        MainMenuState = BaseMenuState.extend({

                init: function(game) {
                        this.game = game;
                        this._super(game);
                },

                create: function() {

                        var self = this;
                        this._super();
                        button = this.game.add.button(1024 / 2, (768 / 2) + 120, 'play_button',
                                null, this, 1, 1, 2);
                        button.anchor.setTo(0.5, 0.5);

                        var style = {
                                font: "bold 40pt courier",
                                fill: "#ffffff",
                                align: "center",
                                stroke: "#ffffff",
                                strokeThickness: 8
                        };

                        s = this.game.add.text(1024 / 2, 768 / 2 - 170, "Select your hero")
                        s.anchor.setTo(0.5, 0.5);


                        var heroCount = 3;
                        // Create our hero stuff here
                        for (var i = 0; i < heroCount; i++) {
                                var sprite = this.game.add.sprite(1024 / 2, 768 / 2 - 10, "player_active", 0);
                                sprite.anchor.setTo(0.5, 0.5);

                                this.game.add.text(1024 / 2, 786 / 2 - 90, "Vaughan").anchor.setTo(0.5, 0.5);

                        }
                        // Setup a hook for logging in
                        this.game.net.registerPacket(PacketTypes.SMSG_LOGIN_RESPONSE,
                                function(response) {
                                        // If we're allowed in
                                        if (response.status == 1) {
                                                self.game.state.add('heroselect', new HeroSelectState(self.game));
                                                self.game.state.start('heroselect');
                                        }
                                });

                        if (Debug.AutoLogin)
                                this.game.net.sendLogin("Vaughan", "Vaughan");



                },


                preload: function() {
                        this._super();
                        this.game.load.image("scroll_bg", "../assets/ui/scroll.png")
                        this.game.load.image("player_active", "../assets/ui/player_active.png");
                        this.game.load.image("player_inactive", "../assets/ui/player_inactive.png");
                        this.game.load.spritesheet('play_button', '../assets/ui/play_button.png', 394, 154);

                }


        });

}();