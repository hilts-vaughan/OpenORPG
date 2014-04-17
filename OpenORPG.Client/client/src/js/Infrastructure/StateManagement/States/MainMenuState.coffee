#
#        This is a state that shows the main menu of the game, it allows the player to work within a UI to reach the gameplay screen.
#
BaseMenuState = require "./../../../Infrastructure/StateManagement/States/BaseMenuState.coffee" 
PacketTypes = require "./../../PacketTypes.coffee"
GameplayState = require "./../../../Game/StateManagement/GameplayState.coffee"
HeroSelectState = require "./../../../Game/StateManagement/HeroSelectState.coffee"

module.exports =
  class MainMenuState extends BaseMenuState
    constructor: (game) ->
      @game = game
      super
      return

    create: ->
      super
      self = this
      button = @game.add.button(1024 / 2, (768 / 2) + 120, "play_button", null, this, 1, 1, 2)
      button.anchor.setTo 0.5, 0.5
      style =
        font: "bold 40pt courier"
        fill: "#ffffff"
        align: "center"
        stroke: "#ffffff"
        strokeThickness: 8

      s = @game.add.text(1024 / 2, 768 / 2 - 170, "Select your hero")
      s.anchor.setTo 0.5, 0.5
      heroCount = 3
      
      # Create our hero stuff here
      i = 0

      while i < heroCount
        sprite = @game.add.sprite(1024 / 2, 768 / 2 - 10, "player_active", 0)
        sprite.anchor.setTo 0.5, 0.5
        @game.add.text(1024 / 2, 786 / 2 - 90, "Vaughan").anchor.setTo 0.5, 0.5
        i++
      
      # Setup a hook for logging in
      @game.net.registerPacket PacketTypes.SMSG_LOGIN_RESPONSE, (response) ->
        
        # If we're allowed in
        if response.status is 1
          self.game.state.add "heroselect", new HeroSelectState(self.game)
          self.game.state.start "heroselect"
        return

      console.log(window.Debug)
      
      if window.Debug.AutoLogin
        username = @getUrlValue("user")
        password = @getUrlValue("pass")
        console.log(username)
        @game.net.sendLogin username, password

      return

    getUrlValue: (VarSearch) ->
      SearchString = window.location.search.substring(1)
      VariableArray = SearchString.split("&")
      i = 0

      while i < VariableArray.length
        KeyValuePair = VariableArray[i].split("=")
        return KeyValuePair[1]  if KeyValuePair[0] is VarSearch
        i++

    preload: ->
      super
      @game.load.image "scroll_bg", "../assets/ui/scroll.png"
      @game.load.image "player_active", "../assets/ui/player_active.png"
      @game.load.image "player_inactive", "../assets/ui/player_inactive.png"
      @game.load.spritesheet "play_button", "../assets/ui/play_button.png", 394, 154
      return