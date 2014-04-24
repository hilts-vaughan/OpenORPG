GameSystem = require('./../../Infrastructure/World/GameSystem.coffee')

module.exports =

	class CombatSystem extends GameSystem

    # We build our combat system here where it's required
    constructor: (@game) ->
      # Hook into some of our network events we might care about here

      # Register for use results
      @game.net.registerPacket PacketTypes.SMSG_SKILL_USE_RESULT, (packet) =>
        victim = @game.entities[packet.targetId]
        user = @game.entities[packet.userId]

        @game.add.tween(victim).to
          tint: 0x7E3517
          alpha: 0.9
        , 200, Phaser.Easing.Linear.None, true, 0, true, true

        # We create the floating text and it will destroy itself in time
        text = new FloatingText(@game, victim, packet.damage)

        # Play the attack animation
        user.playSkillAnimation()

    update: ->

