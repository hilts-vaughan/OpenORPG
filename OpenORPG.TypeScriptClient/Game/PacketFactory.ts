module PacketFactory {

    export function createLoginPacket(user: string, password: string) {
        return {
            opCode: OpenORPG.OpCode.CMSG_LOGIN_REQUEST,
            username: user,
            password: password
        };
    }

    export function createHeroSelectPacket(id: number) {
        return {
            opCode: OpenORPG.OpCode.CMSG_HERO_SELECT,
            heroId: id
        };
    }


    export function createMovementPacket(x: number, y: number, terminate: boolean, direction: OpenORPG.Direction) {
        return {
            opCode: OpenORPG.OpCode.CMSG_MOVEMENT_REQUEST,
            currentPosition: {
                x: x,
                y: y
            },

            terminates: terminate,
            direction: direction
        }
    }

    export function createZoneRequestChange(dir: OpenORPG.Direction) : any {
        return {
            opCode: OpenORPG.OpCode.CMMSG_ZONE_CHANGE,
            direction: dir
        }
    }

    export function createSkillUsePacket(skillId: number, targetId: number) {
        return {
            opCode: OpenORPG.OpCode.CMSG_USE_SKILL,
            skillId: skillId,
            targetId: targetId
        }
    }

    export function createChatPacket(channeld: number, message: string) {
        return {
            opCode: OpenORPG.OpCode.CMSG_CHAT_MESSAGE,
            channelId: channeld,
            message: message
        }
    }



} 